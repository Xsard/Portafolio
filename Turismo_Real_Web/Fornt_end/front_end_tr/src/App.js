import './App.css';
import { Navigation } from "./Components/navbar/navbar";
import { Inicio } from "./Pages/Inicio";
import { FormularioLogin } from "./Components/formulario/form_login";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { FormularioRegistrarse } from './Components/formulario/form_registrarse';
import Footer from './Components/Footer/footer';

function App() {
  return (
    <>
      <div className='page-container'>
        <Router>
          <div className='content-warp'>
            <Navigation />
            <Routes>
              <Route path="/Inicio" element={<Inicio />}></Route>
              <Route path="/" element={<Inicio />}></Route>
              <Route path="/Login" element={<FormularioLogin />}></Route>
              <Route path="/Registrarse" element={<FormularioRegistrarse />}></Route>
            </Routes>
          </div>
        </Router>
        <Footer/>
      </div>
    </>
  );
}

export default App;
