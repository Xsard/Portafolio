import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { NavLink } from "react-router-dom";
import NavDropdown from "react-bootstrap/NavDropdown";
import NavbarBrand from "react-bootstrap/esm/NavbarBrand";
import '../navbar/navbar.css'

export const Navigation = () => {
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
            <NavLink className="nav-link" to="/Login">
              Iniciar sesión
            </NavLink>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};