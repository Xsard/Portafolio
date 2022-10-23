import axios from "axios";
import { useParams } from "react-router-dom";
import { useState } from "react";
import foto_test from "../../imagenes_Dpto/22.jpg"
import '../DeptoComponent/deptovista.css'
import foto_alt from "../../Img/alt.jpg"
import { Form } from "react-bootstrap";
import { useContext } from "react";
import clienteContext from "../../Contexts/ClienteContext";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { Modal, Button } from "react-bootstrap";


const DeptoVista = () => {
    const { id_depto } = useParams()
    const url = `http://localhost:8080/api/v1/test/${id_depto}`;

    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const { usuario, id } = useContext(clienteContext);

    const [idReserva, setIdReserva] = useState('');

    const [nombre_dpto, setNombreDepto] = useState('');
    const [idDepto, setIdDepto] = useState('');
    const [NumeroDepto, setNumeroDepto] = useState('');
    const [capacidad, setCapacidad] = useState('');
    const [tarifa, setTarifa] = useState('');
    const [direccion, setDireccion] = useState('');
    const [id_comuna, setComuna] = useState('');
    const [nombreComuna, setnombreComuna] = useState('');

    const [fechaIda, setFechaIda] = useState('');
    const [fechaVuelta, setFechaVuelta] = useState('');
    const [cantAcompañantes, setAcompañantes] = useState('');

    const MySwal = withReactContent(Swal);

    const handleOnLoad = async (e) => {
        e.preventDefault();
        try {
            const resp = await axios.get(url)
            setDireccion(resp.data.direccion)
            setIdDepto(resp.data.idDepto)
            setNombreDepto(resp.data.nombre_dpto)
            setCapacidad(resp.data.capacidad)
            setNumeroDepto(resp.data.nroDepto)
            setTarifa(resp.data.tarifaDiaria)
            setComuna(resp.data.id_comuna)
            const respComuna = await axios.get(`http://localhost:8080/api/v1/comunaid/${resp.data.id_comuna}`)
            setnombreComuna(respComuna.data.nombre_comuna)
        }
        catch (error) {
            console.log(error.response)
        }
    }

    const handlePostReserva = async (e) => {
        e.preventDefault();
        try {
            if (id === null) {
                MySwal.fire({
                    title: "Debes iniciar sesion para continuar con la reserva",
                    icon: "error"
                }).then((respuesta) => {
                    if (respuesta.isConfirmed) {
                        window.location.replace('/Login');
                    }
                })
            } else {
                //const resp = await axios.post('http://localhost:8080/api/v1/guardarReserva', {
                    //id_dpto: id_depto, id_cliente: id, estado_reserva: "I", estado_pago: "P", check_in: fechaIda,
                    //check_out: fechaVuelta, firma: 0, valor_total: 20000
                //})
            }
        }
        catch (error) {
            console.log(error.response)
        }
    }

    const handleTest = () => {
        handleShow()
        
    }

    return (
        <>
            <div onLoad={handleOnLoad}>
                <div className="divmayor " >
                    <br></br>
                    <div className="row g-lg-2" >
                        <a href="" className="col col-lg-6"><img src={foto_test} style={{ width: "100%", height: "100%" }}></img></a>
                        <div className="col col-lg-6">
                            <div className="row row-cols-2 row-cols-lg-2 g-2">
                                <a href="" className="col"><img src={foto_test} alt={foto_alt} style={{ width: "100%", height: "100%" }}></img></a>
                                <a href="" className="col"><img src={foto_test} alt={foto_alt} style={{ width: "100%", height: "100%" }}></img></a>
                                <a href="" className="col"><img src={foto_test} alt={foto_alt} style={{ width: "100%", height: "100%" }}></img></a>
                                <a href="" className="col"><img src={foto_test} alt={foto_alt} style={{ width: "100%", height: "100%" }}></img></a>
                            </div>
                        </div>
                    </div>

                    <div className="row text ">
                        <div className="card mt-3 cardsinfo" >
                            <div className="card-body">
                                <h3>Departamento: {nombre_dpto}</h3>
                                <p className="card-text">
                                    <b>Numero departamento: </b>{NumeroDepto}<br />
                                    <b>Capacidad: </b>{capacidad}<br />
                                    <b>Direccion: </b>{direccion} <br />
                                    <b>Tarifa: </b>{tarifa} <br />
                                    <b>Comuna: </b>{nombreComuna}
                                </p>
                            </div>
                        </div>
                        <div className="card mt-3 cardsreserv text text-right">
                            <div className="card-body">
                                <h3>Seleccionar fechas</h3>
                                <p className="card-text">
                                    <Form.Group className="form-input mb-3"
                                        style={{ width: "50%" }}
                                        type="date"
                                        id="fechaida"
                                        value={fechaIda}
                                        onChange={(e) => setFechaIda(e.target.value)}>
                                        <Form.Label>Fecha ida</Form.Label>
                                        <Form.Control type="date" placeholder="Ingrese nombres" />
                                    </Form.Group>
                                    <Form.Group className="form-input mb-3 "
                                        style={{ width: "50%" }}
                                        type="date"
                                        id="fechavuelta"
                                        value={fechaVuelta}
                                        onChange={(e) => setFechaVuelta(e.target.value)}>
                                        <Form.Label>Fecha ida</Form.Label>
                                        <Form.Control type="date" placeholder="Ingrese nombres" />
                                    </Form.Group>
                                    <Form.Group className="form-input mb-3 "
                                        style={{ width: "50%" }}
                                        type="text"
                                        id="acompañante"
                                        value={cantAcompañantes}
                                        onChange={(e) => setAcompañantes(e.target.value)}>
                                        <Form.Label>Acompañantes</Form.Label>
                                        <Form.Control type="text" placeholder="Ingrese acompañantes" />
                                    </Form.Group>
                                    <br></br>
                                    <button className="btn btn-primary" onClick={handleTest}>Reserva ahora</button>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>  
        </>
    )
}
export default DeptoVista;