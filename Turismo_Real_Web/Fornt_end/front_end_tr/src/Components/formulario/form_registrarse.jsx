import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Footer from '../Footer/footer';

export const FormularioRegistrarse = () => {
    return (
        <>
            <Form className='mx-auto mt-5 w-25'>
                <Form.Group className="mb-3" controlId="rut_registrarse">
                    <Form.Label>RUT</Form.Label>
                    <Form.Control type="text" placeholder="Ej: 20382647-3" />
                </Form.Group>
                <Form.Group className="mb-3" controlId="nombre_registrarse">
                    <Form.Label>Nombres</Form.Label>
                    <Form.Control type="text" placeholder="Ingrese nombres" />
                </Form.Group>
                <Form.Group className="mb-3" controlId="apellidos_registrarse">
                    <Form.Label>Apellidos</Form.Label>
                    <Form.Control type="text" placeholder="Ingrese apellidos" />
                </Form.Group>
                <Form.Group className="mb-3" controlId="correo_registrarse">
                    <Form.Label>Correo</Form.Label>
                    <Form.Control type="email" placeholder="Ingrese un correo" />
                </Form.Group>
                <Form.Group className="mb-3" controlId="pass_registrarse">
                    <Form.Label>Contraseña</Form.Label>
                    <Form.Control type="password" placeholder="Contraseña" />
                </Form.Group>
                <Form.Group className="mb-3" controlId="pass_repeat_registrarse">
                    <Form.Label>Repite contraseña</Form.Label>
                    <Form.Control type="password" placeholder="Contraseña" />
                </Form.Group>

                <Button variant="primary" type="submit" href=''>
                    Continuar
                </Button>
                <Button variant="danger" type="submit" className='text mx-3' href='Inicio'>
                    Cancelar
                </Button>
            </Form>
        </>
    );
}