import './App.css';
import { Navigation } from "./Components/navbar/navbar";
import { Inicio } from "./Pages/Inicio";
import { FormularioLogin } from "./Components/formulario/form_login";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { FormularioRegistrarse } from './Components/formulario/form_registrarse';
import Footer from './Components/Footer/footer';
import clienteContext from './Contexts/ClienteContext';
import { useState } from 'react';

const initialUsuario = null

function App() {
  const [usuario, setUsuario] = useState(initialUsuario)

  const data = { usuario, setUsuario }
  return (
    <>
      <div className='page-container'>
        <Router>
          <clienteContext.Provider value={data}>
            <div className='content-warp'>
              <Navigation />
              <Routes>
                <Route path='/Inicio' element={<Inicio />}></Route>
                <Route path='/' exact element={<Inicio />}></Route>
                <Route path="/Login" element={<FormularioLogin />}></Route>
                <Route path="/Registrarse" element={<FormularioRegistrarse />}></Route>
              </Routes>
              <Footer />
            </div>
          </clienteContext.Provider>
        </Router >
      </div>

    </>
  );
}
export default App;
