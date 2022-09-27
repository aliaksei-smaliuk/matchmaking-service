import React, {useState} from 'react';

let timeout;
let count = 0;
let requestsPerMinuteGlobal;

function getRandomInt(max) {
    return Math.round(Math.random() * max);
}

const processInterval = async function () {
    count++;
    const request = {
        playerId: `playerId${count}`,
        level: getRandomInt(20),
        cash: getRandomInt(100),
        platform: getRandomInt(2),
        hoursInGame: getRandomInt(1000),
        gameType: getRandomInt(1)
    }
    const body = JSON.stringify(request)
    console.log(body)
    const init = {
        method: 'POST',
        body: body,
        headers: {
            'Content-Type': 'application/json'
        }
    }
    const response = await fetch('/matchmaking', init);
    timeout = setTimeout(processInterval, 60000 / requestsPerMinuteGlobal);
}

export function PlayerGenerator() {
    const [isGenerating, setIsGenerating] = useState(false);
    const [requestsPerMinute, setRequestsPerMinute] = useState(139);

    function handleRequestsPerMinuteChanged(e) {
        setRequestsPerMinute(e.target.value);
    }

    function startProcessing() {
        requestsPerMinuteGlobal = requestsPerMinute;
        processInterval();
        setIsGenerating(true);
    }

    function stopProcessing() {
        clearTimeout(timeout);
        setIsGenerating(false);
    }

    return (
        <div className="row">
            <div className="col">
                {
                    !isGenerating
                        ? <button type="button" className="btn btn-success" onClick={startProcessing}>Start</button>
                        : <button type="button" className="btn btn-danger" onClick={stopProcessing}>Stop</button>
                }
            </div>
            <div className="col">
                <input className="form-control" disabled={isGenerating} value={requestsPerMinute}
                       onChange={handleRequestsPerMinuteChanged}/>
            </div>
        </div>
    );
}