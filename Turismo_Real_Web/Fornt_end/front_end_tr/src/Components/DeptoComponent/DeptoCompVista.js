import axios from "axios";
import { useParams } from "react-router-dom";
import { useState } from "react";
import '../DeptoComponent/deptovista.css'
import { Form } from "react-bootstrap";
import { useContext } from "react";
import clienteContext from "../../Contexts/ClienteContext";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { Modal, Button } from "react-bootstrap";
import { ReactDOM } from "react";
import { useEffect } from "react";



const DeptoVista = () => {
    const { id_depto } = useParams()
    const url = `http://localhost:8080/api/v1/test/${id_depto}`;
    const url_all = `http://localhost:8080/api/v1/fotosDepartamento/${id_depto}`;

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

    const [foto_0, setFoto0] = useState('');
    const [foto_1, setFoto1] = useState('');
    const [foto_2, setFoto2] = useState('');
    const [foto_3, setFoto3] = useState('');
    const [foto_4, setFoto4] = useState('');

    const MySwal = withReactContent(Swal);

    useEffect(() => {
        const CargarFotos = async () => {
            const resp = await axios.get(`http://localhost:8080/api/v1/fotosDepartamento/${id_depto}`)
            setFoto0(resp.data[0])
            setFoto1(resp.data[1])
            setFoto2(resp.data[2])
            console.log(resp.data[3])
        }
        CargarFotos()
    }, [])

    const Cargar = async (e) => {
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

    const test = () => {
        handleShow()
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
                if (fechaIda === '' && fechaVuelta === '') {
                    MySwal.fire({
                        title: "Debe rellenar las fechas",
                        icon: "error"
                    })
                }
                else {
                    if (cantAcompañantes === '' || cantAcompañantes > capacidad) {
                        MySwal.fire({
                            title: "Debe rellenar los acompañantes o no superar la cantidad maxima de la capacidad del departamento",
                            icon: "error"
                        })
                    }
                    else {
                        MySwal.fire({
                            title: "¿Desea agregar transporte?",
                            icon: "info",
                            showDenyButton: true,
                            confirmButtonText: 'Si',
                            denyButtonText: `No`
                        }).then((respuesta) => {
                            if (respuesta.isConfirmed) {
                                let fecha1 = new Date(fechaIda).getTime();
                                let fecha2 = new Date(fechaVuelta).getTime();
                                let diff = fecha2 - fecha1;

                                console.log(diff / (1000 * 60 * 60 * 24))

                                let valorTotal = tarifa * diff / (1000 * 60 * 60 * 24)
                                console.log(valorTotal)

                                const resp = axios.post('http://localhost:8080/api/v1/reserva_pl', {
                                    id_dpto: id_depto, id_cliente: id, estado_reserva: "I", estado_pago: "P", check_in: fechaIda,
                                    check_out: fechaVuelta, firma: 0, valor_total: valorTotal, cantidad_acompañantes: cantAcompañantes, transporte: "S"
                                }).then(resp => {localStorage.setItem('idReserva', resp.data)})                           
                            }
                            else {
                                let fecha1 = new Date(fechaIda).getTime();
                                let fecha2 = new Date(fechaVuelta).getTime();
                                let diff = fecha2 - fecha1;

                                console.log(diff / (1000 * 60 * 60 * 24))

                                let valorTotal = tarifa * diff / (1000 * 60 * 60 * 24)
                                console.log(valorTotal)

                                const resp = axios.post('http://localhost:8080/api/v1/reserva_pl', {
                                    id_dpto: id_depto, id_cliente: id, estado_reserva: "I", estado_pago: "P", check_in: fechaIda,
                                    check_out: fechaVuelta, firma: 0, valor_total: valorTotal, cantidad_acompañantes: cantAcompañantes, transporte: "N"
                                }).then(resp => {localStorage.setItem('idReserva', resp.data)})
                            }
                            MySwal.fire({
                                title: "¿Desea agregar servicios extras?",
                                icon: "info",
                                showDenyButton: true,
                                confirmButtonText: 'Si',
                                denyButtonText: 'No'
                            }).then((respuesta) => {
                                if(respuesta.isConfirmed){
                                    const idReserva1 = localStorage.getItem('idReserva')
                                    window.location.replace(`/ListaServExtra/${idReserva1}`);
                                }
                                else{
                                    MySwal.fire({
                                        title: "Reserva Exitosa",
                                        icon: "success"
                                    }).then((respuesta) => {
                                        if (respuesta.isConfirmed) {
                                            window.location.replace('/Inicio');
                                        }
                                    })
                                }
                                
                            })
                        })
                    }
                }
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
        <div onLoad={Cargar}>
            <div className="divmayor " >
                <br></br>
                <div className="row g-lg-2" >
                    {
                        foto_0 === '' ?
                            <a href="" className="col col-lg-6"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} style={{ width: "100%", height: "100%" }}></img></a> :
                            <a href="" className="col col-lg-6"><img src={require(`../../imagenes_Dpto/${foto_0}.jpg`)} style={{ width: "100%", height: "100%" }}></img></a>
                    }
                    <div className="col col-lg-6">
                        <div className="row row-cols-2 row-cols-lg-2 g-2">
                            {
                                foto_1 === '' ?
                                    <a href="" className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                    <a href="" className="col"><img src={require(`../../imagenes_Dpto/${foto_1}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></a>
                            }
                            {
                                foto_2 === '' ?
                                    <a href="" className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                    <a href="" className="col"><img src={require(`../../imagenes_Dpto/${foto_0}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></a>
                            }
                            {
                                foto_3 === '' ?
                                    <a href="" className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                    <a href="" className="col"><img src={require(`../../imagenes_Dpto/${foto_0}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></a>
                            }
                            {
                                foto_4 === '' ?
                                    <a href="" className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                    <a href="" className="col"><img src={require(`../../imagenes_Dpto/${foto_0}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></a>
                            }

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
                                <button className="btn btn-primary" onClick={handlePostReserva}>Reserva ahora</button>
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