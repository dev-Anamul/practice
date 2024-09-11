import "./App.css";
import { Route, Routes } from "react-router-dom";
import EmailVerification from "./pages/EmailVerification";
import ResetPassword from "./pages/ResetPassword";
import Home from "./pages/Home";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" Component={Home} />
        <Route
          path="/email-verification/:token"
          Component={EmailVerification}
        />
        <Route path="/reset-password/:token" Component={ResetPassword}></Route>
      </Routes>
    </>
  );
}

export default App;
