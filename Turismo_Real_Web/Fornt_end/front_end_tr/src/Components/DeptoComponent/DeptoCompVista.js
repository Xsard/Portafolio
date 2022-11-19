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
import { Link } from "react-router-dom";
import DeptoServiceComp from "./DeptoServicioComponente";

const DeptoVista = () => {

    //usamos el useParams que nos devolvera el parametro que viene en la URL
    const { id_depto } = useParams()

    //variables con las url que llaman al backend
    const url = `http://localhost:8080/api/v1/test/${id_depto}`;
    const url_all = `http://localhost:8080/api/v1/fotosDepartamento/${id_depto}`;

    //llamamos al useContext para obtener los datos de la sesion activa
    const { usuario, id } = useContext(clienteContext);

    //creamos los parametros que se van a utilizar con el useState
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
    const [fecha_actual, setFechaActual] = useState('');



    //guardamos el id del departamento en un localStorage para utilizarlo en otras ventanas
    localStorage.setItem('idDeptoFotos', id_depto)
    let fotos_id = localStorage.getItem('idDeptoFotos')

    //creamos la variable que llamara el Swal Alert
    const MySwal = withReactContent(Swal);

    //creamos un useEffect que se encarga de cargar al iniciar la pagina
    useEffect(() => {
        //llamamos al backend y traemos todas las fotos del departamento
        const CargarFotos = async () => {
            const resp = await axios.get(`http://localhost:8080/api/v1/fotosDepartamento/${id_depto}`)
            //traemos los 5 primeros datos para insertarlos en la pagina principal
            setFoto0(resp.data[0])
            setFoto1(resp.data[1])
            setFoto2(resp.data[2])
            setFoto3(resp.data[3])
            setFoto4(resp.data[4])
        }
        CargarFotos()
    }, [])

    //creamos una funcion que se encarga de traer los datos de los departamentos y cargarlos cuando se inicie
    const Cargar = async (e) => {
        e.preventDefault();
        try {
            // crea un nuevo objeto `Date`
            let today = new Date();
            //devuelve el día del mes (del 1 al 31)
            let day = today.getDate();
            //devuelve el mes (de 0 a 11)
            let month = today.getMonth() + 1;
            //devuelve el año completo
            let year = today.getFullYear();
            //guardamos la fecha
            let tiempo = `${year}-${month}-${day}`
            setFechaActual(tiempo)
            document.getElementById("fechaReserva").min = tiempo
            document.getElementById("fechaReserva2").min = tiempo

            //todos los datos obtenidos de la consulta se almacenan en los useStates creados anteriormente
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

    //creamos la funcion que hara la reserva
    const handlePostReserva = async (e) => {
        e.preventDefault();
        try {

            //comprobamos que el usuario este logueado para hacer la reserva, sino esta logueado se redirigue al login
            if (id === null) {
                MySwal.fire({
                    title: "Debes iniciar sesion para continuar con la reserva",
                    icon: "error"
                }).then((respuesta) => {
                    if (respuesta.isConfirmed) {
                        window.location.replace('/Login');
                    }
                })
                //si esta logueado se comprueba que las fechas no esten vacias
            } else {
                if (fechaIda === '' && fechaVuelta === '') {
                    MySwal.fire({
                        title: "Debe rellenar las fechas",
                        icon: "error"
                    })
                }
                //si las fechas estan, se comprueba que los acompañantes no esten vacios o no superen la capacidad
                else {
                    if (cantAcompañantes === '' || cantAcompañantes > capacidad) {
                        MySwal.fire({
                            title: "Debe rellenar los acompañantes o no superar la cantidad maxima de la capacidad del departamento",
                            icon: "error"
                        })
                    }
                    //Pregunta si desea agregar transporte
                    else {
                        MySwal.fire({
                            title: "¿Desea agregar transporte?",
                            icon: "info",
                            showDenyButton: true,
                            allowOutsideClick: false,
                            confirmButtonText: 'Si',
                            denyButtonText: `No`
                        }).then((respuesta) => {
                            //comienza la inserccion
                            if (respuesta.isConfirmed) {
                                //hace un calculo con las fechas para obtener la cantidad de dias
                                let fecha1 = new Date(fechaIda).getTime();
                                let fecha2 = new Date(fechaVuelta).getTime();
                                let diff = fecha2 - fecha1;

                                console.log(diff / (1000 * 60 * 60 * 24))

                                //genera el valor total con la tarifa diaria multiplicando los dias de estadia
                                let valorTotal = tarifa * diff / (1000 * 60 * 60 * 24)
                                console.log(valorTotal)

                                //inserta la reserva en la base de datos
                                const resp = axios.post('http://localhost:8080/api/v1/reserva_pl', {
                                    id_dpto: id_depto, id_cliente: id, estado_reserva: "I", estado_pago: "P", check_in: fechaIda,
                                    check_out: fechaVuelta, firma: 0, valor_total: valorTotal, cantidad_acompañantes: cantAcompañantes, transporte: "S"
                                }).then(resp => { localStorage.setItem('idReserva', resp.data) })
                            }
                            else {
                                //hace un calculo con las fechas para obtener la cantidad de dias
                                let fecha1 = new Date(fechaIda).getTime();
                                let fecha2 = new Date(fechaVuelta).getTime();
                                let diff = fecha2 - fecha1;

                                console.log(diff / (1000 * 60 * 60 * 24))

                                //genera el valor total con la tarifa diaria multiplicando los dias de estadia
                                let valorTotal = tarifa * diff / (1000 * 60 * 60 * 24)
                                console.log(valorTotal)

                                //inserta la reserva en la base de datos
                                const resp = axios.post('http://localhost:8080/api/v1/reserva_pl', {
                                    id_dpto: id_depto, id_cliente: id, estado_reserva: "I", estado_pago: "P", check_in: fechaIda,
                                    check_out: fechaVuelta, firma: 0, valor_total: valorTotal, cantidad_acompañantes: cantAcompañantes, transporte: "N"
                                }).then(resp => { localStorage.setItem('idReserva', resp.data) })
                            }
                            //pregunta acerca de los servicios extras
                            MySwal.fire({
                                title: "¿Desea agregar tours?",
                                icon: "info",
                                showDenyButton: true,
                                allowOutsideClick: false,
                                confirmButtonText: 'Si',
                                denyButtonText: 'No'
                            }).then((respuesta) => {
                                //si la respuesta es si, es redigido a la lista de los servicios extras
                                if (respuesta.isConfirmed) {
                                    const idReserva1 = localStorage.getItem('idReserva')
                                    localStorage.setItem('res_tour', idReserva1)
                                    localStorage.setItem('depto_tour', id_depto)
                                    localStorage.setItem('ocultarBtn', 0)
                                    window.location.replace(`/mostrartour/${idReserva1}`);
                                }
                                else {
                                    //sino no desea servicios extras lo redirigue al inicio
                                    MySwal.fire({
                                        title: "¿Desea agregar servicios extras?",
                                        icon: "info",
                                        showDenyButton: true,
                                        allowOutsideClick: false,
                                        confirmButtonText: 'Si',
                                        denyButtonText: 'No'
                                    }).then((respuesta) => {
                                        if (respuesta.isConfirmed) {
                                            const idReserva1 = localStorage.getItem('idReserva')
                                            window.location.replace(`/ListaServExtra/${idReserva1}`);
                                        } else {
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

    return (
        <>
            <div onLoad={Cargar}>
                <div className="divmayor " >
                    <br></br>
                    <div className="row g-lg-2" >
                        {
                            foto_0 === '' || foto_0 === undefined ?
                                <a to={`/mostrarFotos/${idDepto}`} className="col col-lg-6"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} style={{ width: "100%", height: "100%" }}></img></a> :
                                <Link to={`/mostrarFotos/${idDepto}`} className="col col-lg-6"><img src={require(`../../imagenes_Dpto/${foto_0}.jpg`)} style={{ width: "100%", height: "100%" }}></img></Link>
                        }
                        <div className="col col-lg-6">
                            <div className="row row-cols-2 row-cols-lg-2 g-2">
                                {
                                    foto_1 === '' || foto_1 === undefined ?
                                        <a to={`/mostrarFotos/${idDepto}`} className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                        <Link to={`/mostrarFotos/${idDepto}`} className="col"><img src={require(`../../imagenes_Dpto/${foto_1}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></Link>
                                }
                                {
                                    foto_2 === '' || foto_2 === undefined ?
                                        <a to={`/mostrarFotos/${idDepto}`} className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                        <Link to={`/mostrarFotos/${idDepto}`} className="col"><img src={require(`../../imagenes_Dpto/${foto_2}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></Link>
                                }
                                {
                                    foto_3 === '' || foto_3 === undefined ?
                                        <a to={`/mostrarFotos/${idDepto}`} className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                        <Link to={`/mostrarFotos/${idDepto}`} className="col"><img src={require(`../../imagenes_Dpto/${foto_3}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></Link>
                                }
                                {
                                    foto_4 === '' || foto_4 === undefined ?
                                        <a to={`/mostrarFotos/${idDepto}`} className="col"><img src={"https://data.pixiz.com/output/user/frame/preview/400x400/1/3/3/9/3069331_726cc.jpg"} alt={""} style={{ width: "100%", height: "100%" }}></img></a> :
                                        <Link to={`/mostrarFotos/${idDepto}`} className="col"><img src={require(`../../imagenes_Dpto/${foto_4}.jpg`)} alt={""} style={{ width: "100%", height: "100%" }}></img></Link>
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
                                        <Form.Control type="date" placeholder="Ingrese nombres" id="fechaReserva" min="2022-11-01" />
                                    </Form.Group>
                                    <Form.Group className="form-input mb-3 "
                                        style={{ width: "50%" }}
                                        type="date"
                                        id="fechavuelta"
                                        value={fechaVuelta}
                                        onChange={(e) => setFechaVuelta(e.target.value)}>
                                        <Form.Label>Fecha vuelta</Form.Label>
                                        <Form.Control type="date" placeholder="Ingrese nombres" id="fechaReserva2" min="2022-11-01" />
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
                    <br></br>
                    <div>
                        <h3 className="text text-center">Servicios incluidos en el departamento</h3>
                        <DeptoServiceComp></DeptoServiceComp>
                    </div>
                </div>

            </div>
        </>
    )
}
export default DeptoVista;