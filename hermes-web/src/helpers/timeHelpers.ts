// src/helpers/timeHelpers.ts

export function formatTimestamp(ms?: number): string {
    if (ms === undefined) return '00:00';
    const seconds = Math.floor(ms / 1000);
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;

    return (
        minutes.toString().padStart(2, '0') +
        ':' +
        remainingSeconds.toString().padStart(2, '0')
    );
}
