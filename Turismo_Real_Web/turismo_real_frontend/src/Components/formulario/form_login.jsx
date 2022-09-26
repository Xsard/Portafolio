import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Footer from '../Footer/footer';
import './form_login.css';

export const FormularioLogin = () => {

  return (
    <>
      <div id='body'>
        <Form className='mx-auto mt-5 w-25' >
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Correo</Form.Label>
            <Form.Control type="email" placeholder="Ingrese un correo" />
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Contraseña</Form.Label>
            <Form.Control type="password" placeholder="Contraseña" />
          </Form.Group>
          <Button variant="primary" type="submit" className='text mx-1'>
            Acceder
          </Button>
          <Button variant="primary" type="submit" className='text mx-3' href='Registrarse'>
            Registrarse
          </Button>
        </Form>
      </div>
      <Footer />
    </>
  );
} 
