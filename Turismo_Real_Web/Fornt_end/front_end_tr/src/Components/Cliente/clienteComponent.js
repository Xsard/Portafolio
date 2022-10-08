import { useState } from "react";
import Form from 'react-bootstrap/Form';
import axios from "axios";
const url = "http://localhost:8080/api/v1/registrarse"

const Registrarse = () => {
    const [rut, setRut] = useState('');
    const [nombres, setNombres] = useState('');
    const [apellidos, setApellidos] = useState('');
    const [correo, setCorreo] = useState('');
    const [telefono, setTelefono] = useState('');
    const [contraseña, setContraseña] = useState('');
    const [repContraseña, setRepContraseña] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const resp = await axios.post(url, {
                email: correo, pass: contraseña, fono: telefono,
                rut: rut, nombre: nombres, apellido: apellidos
            })
            console.log(resp.data)
        } catch (error) {
            console.log(error.response)
        }
    }

    return (
        //el onSubmit toma el boton
        <div className="mx-auto">
            <br></br>
            <h2 className="text-center">Registrate en Turismo Real</h2>
            <form className="form mx-auto mt-5 w-25" onSubmit={handleSubmit}>
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="text"
                        id="rut"
                        value={rut}
                        onChange={(e) => setRut(e.target.value)}>
                        <Form.Label>RUT</Form.Label>
                        <Form.Control type="text" placeholder="Ej: 20382647-3" />
                    </Form.Group>
                </div>
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="text"
                        id="nombres"
                        value={nombres}
                        onChange={(e) => setNombres(e.target.value)}>
                        <Form.Label>Nombres</Form.Label>
                        <Form.Control type="text" placeholder="Ingrese nombres" />
                    </Form.Group>
                </div>
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="text"
                        id="apellidos"
                        value={apellidos}
                        onChange={(e) => setApellidos(e.target.value)}>
                        <Form.Label>Apellidos</Form.Label>
                        <Form.Control type="text" placeholder="Ingrese apellidos" />
                    </Form.Group>
                </div>
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="text"
                        id="correo"
                        value={correo}
                        onChange={(e) => setCorreo(e.target.value)}>
                        <Form.Label>Correo</Form.Label>
                        <Form.Control type="email" placeholder="Ingrese un correo" />
                    </Form.Group>
                </div>
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="text"
                        id="telefono"
                        value={telefono}
                        onChange={(e) => setTelefono(e.target.value)}>
                        <Form.Label>Telefono</Form.Label>
                        <Form.Control type="text" placeholder="Ej: 99999999" />
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
                <div className="form-row mb-3">
                    <Form.Group className="form-input mb-3"
                        type="password"
                        id="repContraseña"
                        value={repContraseña}
                        onChange={(e) => setRepContraseña(e.target.value)}>
                        <Form.Label>Repite contraseña</Form.Label>
                        <Form.Control type="password" placeholder="Contraseña" />
                    </Form.Group>
                </div>
                <br></br>
                <button type='submit' className='btn btn-primary'>
                    Registrarse
                </button>
            </form>
        </div>
    )
};
export default Registrarse;