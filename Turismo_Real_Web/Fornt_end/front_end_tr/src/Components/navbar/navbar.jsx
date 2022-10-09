import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { NavLink } from "react-router-dom";
import NavDropdown from "react-bootstrap/NavDropdown";
import NavbarBrand from "react-bootstrap/esm/NavbarBrand";
import '../navbar/navbar.css'
import { useContext } from "react";
import clienteContext from "../../Contexts/ClienteContext";

export const Navigation = () => {

  const {usuario, setUsuario} = useContext(clienteContext);
  return (
    <Navbar
      className="sticky-top"
      collapseOnSelect
      expand="lg"
      bg=""
      variant="dark"
    >
      <Container>
        <NavbarBrand href="Inicio"> Turismo Real</NavbarBrand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto">
          </Nav>
          <Nav>

            {usuario
              ? <NavDropdown
                title={usuario}
                id="collasible-nav-dropdown"
              >
                <NavDropdown.Item href="#">Reservas</NavDropdown.Item>
                <NavDropdown.Item href="#">Cerrar Sesion</NavDropdown.Item>
              </NavDropdown>

              : <NavLink className="nav-link" to="/Login">
                Iniciar sesión
              </NavLink>
            }
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};