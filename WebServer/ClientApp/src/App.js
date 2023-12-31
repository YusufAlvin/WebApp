import * as signalR from '@microsoft/signalr';
import React from 'react';
import './App.css';

const data = {
  sequences: [
    {
      id: 1,
      position: [8,8]
    },
    {
      id: 2,
      position: [1,1]
    },
    {
      id: 3,
      position: [5,1]
    },
    {
      id: 4,
      position: [2,6]
    },
  ]
}

const URL_LOCATION = `http://${window.location.host}`;
const runSequenceApi = `${URL_LOCATION}/api/sequence/run-sequence`;
const pauseSequenceApi = `${URL_LOCATION}/api/sequence/pause-sequence`;
const resumeSequenceApi = `${URL_LOCATION}/api/sequence/resume-sequence`;
const stopSequenceApi = `${URL_LOCATION}/api/sequence/stop-sequence`;
const hubApi = `${URL_LOCATION}/hub/instrument-hub`;

function App() {
  const [status, setStatus] = React.useState("Idle");

  React.useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubApi)
      .build();

    connection.on("OnSequenceProgress", (data) => {
      console.log('OnSequenceProgress', data);
      setStatus(data.status);
    });

    connection.start();
  }, []);

  const handleRunClick = async () => {
    fetch(runSequenceApi, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });
  };

  const handlePauseClick = async () => {
    await fetch(pauseSequenceApi, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: ''
    });
  };

  const handleResumeClick = async () => {
    await fetch(resumeSequenceApi, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: ''
    });
  };

  const handleStopClick = async () => {
    await fetch(stopSequenceApi, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: ''
    });
  };

  return (
    <div className="App">
      <div className='row-1'>Status: {status}</div>
      <div className='row-2'>
        {(status === "Idle" || status === "Stopped") ? (<button onClick={handleRunClick}>Run</button>) : <></>}
        {status === "Running" ? (<button onClick={handlePauseClick}>Pause</button>) : <></>}
        {status === "Paused" ? (<button onClick={handleResumeClick}>Resume</button>) : <></>}
        {(status === "Running" || status === "Paused") ? (<button onClick={handleStopClick}>Stop</button>) : <></>}
      </div>
    </div>
  );
}

export default App;
