import './App.css';
import { Navigation } from "./Components/navbar/navbar";
import  ClienteComponent  from "./Components/ClienteComponent/ClienteComponente";
import { Inicio } from "./Pages/Inicio";
import { FormularioLogin } from "./Components/formulario/form_login";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

function App() {
  return (
    <>
    <Router>
      <Navigation />
      <ClienteComponent />
      <Routes>
        <Route path="/Inicio" element={<Inicio />}></Route>
        <Route path="/Login" element={<FormularioLogin/>}></Route>
      </Routes>
    </Router>
    </>
  );
}

export default App;
