import { useContext, useState } from "react";
import Button from 'react-bootstrap/Button';
import axios from "axios";
import Form from 'react-bootstrap/Form';
import clienteContext from "../../Contexts/ClienteContext";


const url = "http://localhost:8080/api/v1/login"

const Login = () => {
    const {usuario, setUsuario} = useContext(clienteContext);

    const [correo, setCorreo] = useState('');
    const [contraseña, setContraseña] = useState('');
    

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const resp = await axios.post(url, { email: correo, pass: contraseña })
            console.log(resp.data)
            if (!resp.data === 0) {
                console.log("no sapa na")
            } else {
                console.log("si sapo algo");
                console.log(correo)
                setUsuario(correo)
                console.log(usuario)
                window.location.replace('/Inicio');
                 

            }
        } catch (error) {
            console.log(error.response)
        }
    }

    return (
        <>
            <div className="mx-auto">
                <br></br>
                <h2 className="text-center">Iniciar sesión</h2>
                <form className="form mx-auto mt-5 w-25" onSubmit={handleSubmit}>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="rut"
                            value={correo}
                            onChange={(e) => setCorreo(e.target.value)}>
                            <Form.Label>Correo</Form.Label>
                            <Form.Control type="email" placeholder="Ingrese un correo" id="correo_login"/>
                        </Form.Group>
                    </div>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="password"
                            id="contraseña"
                            value={contraseña}
                            onChange={(e) => setContraseña(e.target.value)}>
                            <Form.Label>Contraseña</Form.Label>
                            <Form.Control type="password" placeholder="Contraseña" />
                        </Form.Group>
                    </div>
                    <button type='submit' className='btn btn-primary'>
                        Iniciar Sesion
                    </button>
                    <Button variant="primary" type="submit" className='text mx-3' href='Registrarse'>
                        Registrarse
                    </Button>
                </form>
            </div>
        </>
    );
};
export default Login;
