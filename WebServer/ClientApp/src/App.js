import './App.css';

const data = {
  sequence: [
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
const runSequenceApi = `${URL_LOCATION}/sequence/run-sequence`;
const pauseSequenceApi = `${URL_LOCATION}/sequence/pause-sequence`;
const resumeSequenceApi = `${URL_LOCATION}/sequence/resume-sequence`;
const stopSequenceApi = `${URL_LOCATION}/sequence/stop-sequence`;

function App() {
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
      <button onClick={handleRunClick}>Run</button>
      <button onClick={handlePauseClick}>Pause</button>
      <button onClick={handleResumeClick}>Resume</button>
      <button onClick={handleStopClick}>Stop</button>
    </div>
  );
}

export default App;
