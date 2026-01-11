// src/helpers/captionHelpers.ts

/** Minimal shape of a caption item coming from the backend */
export interface CaptionSourceSentence {
    sentencePos: number;
    originalText: string;
    startTimeMs?: number;
    endTimeMs?: number;
}

/** One piece of text coming from a single original caption entry */
export interface CaptionSentencePart {
    chunkIndex: number; // index into the original "text" array (same as sentencePos)
    startTimeMs?: number;
    endTimeMs?: number;
    text: string;
}

/** A logical sentence formed by one or more caption parts */
export interface CaptionSentenceUI {
    sentenceIndex: number;
    parts: CaptionSentencePart[];
}

/** Flat view of parts with sentence metadata, used for time lookup */
export interface CaptionPartTimed extends CaptionSentencePart {
    sentenceIndex: number;
    partIndexInSentence: number;
}

/**
 * Per-part word index based on that part's own [start, end) window.
 */
export function getActiveWordIndexForPart(
    part: CaptionSentencePart,
    currentTimeMs: number,
    wordCount: number
): number | null {
    if (wordCount === 0) return null;
    if (part.startTimeMs == null || part.endTimeMs == null) return null;

    const start = part.startTimeMs;
    const end = part.endTimeMs;
    if (end <= start) return null;

    if (currentTimeMs < start) return 0;
    if (currentTimeMs > end) return wordCount - 1;

    const totalDuration = end - start;
    const elapsed = currentTimeMs - start;

    let relative = elapsed / totalDuration;
    if (relative < 0) relative = 0;
    if (relative > 1) relative = 1;

    let idx = Math.floor(relative * wordCount);
    if (idx >= wordCount) idx = wordCount - 1;
    return idx;
}

/**
 * Group a flat list of caption chunks into logical sentences (paragraph-like)
 * and split each chunk into parts with their own approximate time windows.
 *
 * - Handles decimal dots like "4.5" (even across chunks) so they DON'T end sentences.
 * - Fixes edge case where a closing quote like ” or " starts the next sentence
 *   after ? / ! / . and should visually belong to the previous sentence.
 */
export function groupCaptionsIntoSentences(
    chunks: CaptionSourceSentence[]
): { sentences: CaptionSentenceUI[]; flatParts: CaptionPartTimed[] } {
    const sentences: CaptionSentenceUI[] = [];
    let flatParts: CaptionPartTimed[] = [];

    if (!chunks || chunks.length === 0) {
        return { sentences, flatParts };
    }

    let currentParts: CaptionSentencePart[] = [];
    let sentenceIndex = 0;

    function flushCurrentSentence() {
        if (currentParts.length === 0) return;

        // Trim leading whitespace of the first part
        const first = currentParts[0];
        currentParts[0] = {
            chunkIndex: first.chunkIndex,
            startTimeMs: first.startTimeMs,
            endTimeMs: first.endTimeMs,
            text: first.text.replace(/^\s+/, '')
        };

        const sentence: CaptionSentenceUI = {
            sentenceIndex,
            parts: currentParts
        };
        sentences.push(sentence);

        sentenceIndex++;
        currentParts = [];
    }

    function isWhitespace(ch: string): boolean {
        return ch === ' ' || ch === '\t' || ch === '\n' || ch === '\r';
    }

    function isDigit(ch: string): boolean {
        return ch >= '0' && ch <= '9';
    }

    /**
     * Decide if the '.' at (text[dotPos]) should be treated as a decimal point
     * rather than as an end-of-sentence:
     * - previous non-space is a digit AND
     * - next non-space (in this chunk or next chunk) is a digit.
     */
    function looksLikeDecimalDot(
        text: string,
        dotPos: number,
        chunkIndex: number
    ): boolean {
        let prevChar: string | null = null;
        for (let j = dotPos - 1; j >= 0; j--) {
            const c = text.charAt(j);
            if (isWhitespace(c)) continue;
            prevChar = c;
            break;
        }
        if (prevChar === null || !isDigit(prevChar)) return false;

        let nextChar: string | null = null;
        for (let k = dotPos + 1; k < text.length; k++) {
            const c2 = text.charAt(k);
            if (isWhitespace(c2)) continue;
            nextChar = c2;
            break;
        }

        if (nextChar === null && chunkIndex + 1 < chunks.length) {
            const nextText = chunks[chunkIndex + 1].originalText || '';
            for (let z = 0; z < nextText.length; z++) {
                const c3 = nextText.charAt(z);
                if (isWhitespace(c3)) continue;
                nextChar = c3;
                break;
            }
        }

        if (nextChar === null || !isDigit(nextChar)) return false;
        return true;
    }

    // ---------------- main pass: build sentences ----------------
    for (let i = 0; i < chunks.length; i++) {
        const chunk = chunks[i];
        const text = chunk.originalText || '';
        if (!text || text.trim().length === 0) continue;

        // First, split this chunk's text into local pieces (by punctuation)
        const localPieces: { text: string; endsSentence: boolean }[] = [];
        let buffer = '';

        for (let pos = 0; pos < text.length; pos++) {
            const ch = text.charAt(pos);
            buffer += ch;

            // Treat "..." as a single non-terminating token
            if (
                ch === '.' &&
                pos + 2 < text.length &&
                text.charAt(pos + 1) === '.' &&
                text.charAt(pos + 2) === '.'
            ) {
                buffer += text.charAt(pos + 1) + text.charAt(pos + 2);
                pos += 2;
                continue;
            }

            let endsSentence = false;

            if (ch === '.') {
                if (!looksLikeDecimalDot(text, pos, i)) {
                    endsSentence = true;
                }
            } else if (ch === '!' || ch === '?') {
                endsSentence = true;
            }

            if (endsSentence) {
                localPieces.push({ text: buffer, endsSentence: true });
                buffer = '';
            }
        }

        const tail = buffer;
        if (tail && tail.trim().length > 0) {
            localPieces.push({ text: tail, endsSentence: false });
        }

        if (localPieces.length === 0) continue;

        // Distribute this chunk's time range across its local pieces
        const chunkStart = chunk.startTimeMs;
        const chunkEnd = chunk.endTimeMs;
        const pieceTimings: { start?: number; end?: number }[] = [];

        if (
            typeof chunkStart === 'number' &&
            typeof chunkEnd === 'number' &&
            chunkEnd > chunkStart
        ) {
            const totalDuration = chunkEnd - chunkStart;
            let totalChars = 0;
            for (let li = 0; li < localPieces.length; li++) {
                totalChars += localPieces[li].text.length;
            }
            let accChars = 0;
            for (let li2 = 0; li2 < localPieces.length; li2++) {
                const piece = localPieces[li2];
                const len = piece.text.length;
                const start =
                    chunkStart +
                    Math.floor(totalDuration * (accChars / (totalChars || 1)));
                const end =
                    li2 === localPieces.length - 1
                        ? chunkEnd
                        : chunkStart +
                          Math.floor(
                              totalDuration *
                                  ((accChars + len) / (totalChars || 1))
                          );
                pieceTimings.push({ start, end });
                accChars += len;
            }
        } else {
            for (let li3 = 0; li3 < localPieces.length; li3++) {
                pieceTimings.push({ start: undefined, end: undefined });
            }
        }

        // Turn local pieces into parts and assign them to sentences
        for (let li4 = 0; li4 < localPieces.length; li4++) {
            const piece2 = localPieces[li4];
            const timing = pieceTimings[li4];

            const part: CaptionSentencePart = {
                chunkIndex: chunk.sentencePos,
                startTimeMs: timing.start,
                endTimeMs: timing.end,
                text: piece2.text
            };
            currentParts.push(part);

            if (piece2.endsSentence) {
                flushCurrentSentence();
            }
        }
    }

    if (currentParts.length > 0) {
        flushCurrentSentence();
    }

    // ---------------- fix stray closing quotes across sentences ----------------
    function isClosingQuote(ch: string): boolean {
        return (
            ch === '"' ||
            ch === '”' ||
            ch === '’' ||
            ch === '\'' ||
            ch === '»'
        );
    }

    for (let si = 0; si < sentences.length - 1; si++) {
        const prevSentence = sentences[si];
        const nextSentence = sentences[si + 1];

        if (!prevSentence.parts.length || !nextSentence.parts.length) continue;

        const lastPart = prevSentence.parts[prevSentence.parts.length - 1];
        const firstPart = nextSentence.parts[0];

        if (!lastPart.text || !firstPart.text) continue;

        const trimmedLast = lastPart.text.replace(/\s+$/, '');
        if (trimmedLast.length === 0) continue;
        const lastChar = trimmedLast.charAt(trimmedLast.length - 1);
        if (lastChar !== '.' && lastChar !== '!' && lastChar !== '?') continue;

        const raw = firstPart.text;
        let idx = 0;
        let prefix = '';
        let sawQuote = false;

        while (idx < raw.length) {
            const c = raw.charAt(idx);
            if (isWhitespace(c)) {
                prefix += c;
                idx++;
                continue;
            }
            if (isClosingQuote(c)) {
                prefix += c;
                sawQuote = true;
                idx++;
                continue;
            }
            break;
        }

        if (!sawQuote) continue;

        // Move the prefix (e.g. ”) from next sentence to the end of the previous one
        lastPart.text = lastPart.text + prefix;
        firstPart.text = raw.substring(idx);
    }

    // ---------------- rebuild flatParts from adjusted sentences ----------------
    flatParts = [];
    for (let sIdx = 0; sIdx < sentences.length; sIdx++) {
        const s = sentences[sIdx];
        for (let pIdx = 0; pIdx < s.parts.length; pIdx++) {
            const p = s.parts[pIdx];
            flatParts.push({
                chunkIndex: p.chunkIndex,
                startTimeMs: p.startTimeMs,
                endTimeMs: p.endTimeMs,
                text: p.text,
                sentenceIndex: sIdx,
                partIndexInSentence: pIdx
            });
        }
    }

    return { sentences, flatParts };
}
