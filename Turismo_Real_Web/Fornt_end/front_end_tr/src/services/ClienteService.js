import axios from "axios";

export const ingresarUsuario = async (correo, contraseña, telefono, rut, nombres, apellidos) => {
    try {
        const resp = await axios.post('http://localhost:8080/api/v1/registrarse', {
            email: correo, pass: contraseña, fono: telefono,
            rut: rut, nombre: nombres, apellido: apellidos
        })
    } catch (error) {
        console.log(error)
    }
}

export const ValidarLogin = async (correoConsulta, code) => {
    try {
        const resp = await axios.post('http://localhost:8080/api/v1/AutRegistrarse', {
            email: correoConsulta,
            code: code
        })
    } catch (error) {
        console.log(error)
    }
}