import "./App.css";
import { exportExcelFileHandler } from "./utils/excel-sheet-download";

function App() {
  return (
    <>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={exportExcelFileHandler}>Export Excel File</button>
      </div>
    </>
  );
}

export default App;
