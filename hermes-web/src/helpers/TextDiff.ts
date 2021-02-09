interface Phrase
{
    score: number,
    cStart: number,
    cEnd: number,
    oStart: number,
    oEnd: number,
    text: string,
    oOrder: number,
    cOrder: number,
    overlappedInOriginal: boolean,
    overlappedInChanged: boolean,
    wrongOrder: boolean
}

export class TextDiffer
{
    original: string;
    changed: string;
    A: string[];
    B: string[];
    state: number[];
    scores: Phrase[];

    constructor(original: string, changed: string)
    {
        this.original = original;
        this.changed = changed;
        this.A = this.original.split(' ');
        this.B = this.changed.split(' ');
        this.state = [];
        this.scores = [];
    }
    scorePhrases()
    {
        for(let i = 0; i < this.B.length; i++)
        {
            for (let j = 0; j < this.A.length; j++)
            {
                if (this.B[i] == this.A[j])
                {
                    var k = 1;
                    var ts = this.B[i];
                    for (; (i + k < this.B.length && j + k < this.A.length) && (this.B[i + k] == this.A[j + k]); k++)
                    {
                        ts += ' ' + this.B[i + k];
                    }
                    this.scores.push({
                        score: k,
                        cStart: i,
                        cEnd: i + k - 1,
                        oStart: j,
                        oEnd: j + k - 1,
                        text: ts,
                        oOrder: 0,
                        cOrder: 0,
                        overlappedInOriginal: false,
                        overlappedInChanged: false,
                        wrongOrder: false});
                }
            }
        }
    }
    checkOrder()
    {
        var i = 0;
        this.scores.sort((x, y) => x.oStart - y.oStart);
        this.scores.filter(x => x.overlappedInChanged == false && x.overlappedInOriginal == false).forEach(x => x.oOrder = ++i);
        i = 0;
        this.scores.sort((x, y) => x.cStart - y.cStart);
        this.scores.filter(x => x.overlappedInChanged == false && x.overlappedInOriginal == false).forEach(x => x.cOrder = ++i);

        this.scores.forEach(x =>
            {
                if (this.scores.filter(y => ((x.cOrder < y.cOrder && x.oOrder > y.oOrder) || (x.cOrder > y.cOrder && x.oOrder < y.oOrder)) && x.score < y.score).length > 0) x.wrongOrder = true;
            });
    }
    checkOverlappInOriginal()
    {
        this.scores.forEach(x =>
        {
            if (this.scores.filter(y => x.oStart <= y.oEnd && x.oEnd >= y.oStart && x.score < y.score).length > 0) x.overlappedInOriginal = true;
        });
    }
    checkOverlappInChanged()
    {
        this.scores.forEach(x =>
        {
            if (this.scores.filter(y => x.cStart <= y.cEnd && x.cEnd >= y.cStart && x.score < y.score).length > 0) x.overlappedInChanged = true;
        });
    }
    setChangedWords()
    {
        var last: Phrase|null = null;
        this.state = new Array<number>(this.B.length);
        if (this.state.length > 0)
        {
            this.scores
                .filter(x => x.overlappedInOriginal == false && x.overlappedInChanged == false && x.wrongOrder == false)
                .sort((x, y) => x.cStart - y.cStart)
                .forEach((i: Phrase) => 
                {
                    for (var j = i.cStart; j <= i.cEnd; j++)
                    {
                        if (last == null || i.oStart > last.oEnd) this.state[j] = 1;
                        if (last == null)
                        {
                            if (i.cStart == 0 && i.oStart > 0 && j == i.cStart) this.state[j]++;
                        }
                        else
                        {
                            if (i.cStart - last.cEnd == 1 && i.oStart - last.oEnd > 1 && j == i.cStart) this.state[j]++;
                        }
                    }
                    last = i;
                })
        }
    }
    calculateDiff()
    {
        this.scorePhrases();
        this.checkOverlappInOriginal();
        this.checkOverlappInChanged();
        this.checkOrder();
        this.setChangedWords();
    }
    getDiff()
    {
        this.calculateDiff();
        
        var resultText: {
            status: number,
            text: string
        }[] = [];

        for (var i = 0; i < this.state.length; i++)
        {
            if (this.state[i] == 2)
            {
                resultText.push({status: 2, text: " *"});
                resultText.push({status: 1, text: this.B[i]});
            }
            else if (this.state[i] == 1)
            {
                resultText.push({status: 1, text: this.B[i]});
            }
            else
            {
                resultText.push({status: 0, text: this.B[i]});
            }
        }
        
        return resultText;
    }
}