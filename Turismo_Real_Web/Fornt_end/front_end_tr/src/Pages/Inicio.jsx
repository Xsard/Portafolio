import { useContext } from "react";
import { CarouselInicio } from "../Components/Carousel/carousel";
import DeptoComponent from '../Components/DeptoComponent/DeptoComponente';
import clienteContext from "../Contexts/ClienteContext";
import "../Pages/Inicio.css"
import buscar from "../Img/buscar.png"
import axios from "axios";
import { useState } from "react";
import { useEffect } from "react";
import DeptoFiltro from "../Components/DeptoComponent/DeptoFiltrado";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import logo from "../Img/turismoRealLetras.png"

export const Inicio = () => {
    const { usuario, setUsuario } = useContext(clienteContext);
    const [listaComuna, setListacomuna] = useState([])
    const [idcomuna, setIdComuna] = useState('')
    const [fecha_actual, setFechaActual] = useState('')
    const [fechaIda, setFechaIda] = useState('');
    const [fechaVuelta, setFechaVuelta] = useState('');
    const [Acompañantes, setAcompañantes] = useState('');
    const MySwal = withReactContent(Swal);

    const fechaIdaHandle = fecha_ida => {
        localStorage.setItem("fecha_ida", fecha_ida)
        setFechaIda(fecha_ida)
    }
    const fechaVueltaHandle = fecha_vuelta => {
        localStorage.setItem("fecha_vuelta", fecha_vuelta)
        setFechaVuelta(fecha_vuelta)
    }

    const handleAcompañante = acomp => {
        const aco = acomp.replace(/\D/g, '');
        localStorage.setItem("acompañante", aco)
        setAcompañantes(aco)
    }

    const cargarComuna = (ev) => {
        setIdComuna(ev.target.value)
        localStorage.setItem("id_com", ev.target.value)
    }

    const redireccion = () => {
        let id_Com = localStorage.getItem("id_com")
        if (id_Com === "Comuna") {
            MySwal.fire({
                title: "Debe ingresar una comuna",
                icon: "error"
            })
        } else {
            if (fechaIda === '' || fechaVuelta === '' || Acompañantes === '') {
                MySwal.fire({
                    title: "Debe completar todos los datos",
                    icon: "error"
                })
            } else {
                window.location.replace('/DeptoFiltrado');
            }
        }

    }
    const cargar = (e) => {
        e.preventDefault();
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
        document.getElementById("fechaReserva").min = tiempo
        document.getElementById("fechaReserva2").min = tiempo
    }

    useEffect(() => {
        const cargarComuna = async () => {
            const resp = await axios.get("http://localhost:8080/api/v1/comunas")
            //resp.data.forEach((comu)=>{
            //console.log(comu.nombre_comuna)
            //})
            setListacomuna(resp.data)

        }
        cargarComuna()
    }, [])

    function fotoNorte() {
        document.getElementById("div_fotos").style.backgroundImage = "url('https://media.vogue.mx/photos/61aa2ef47a5d7b17735e8e33/16:9/w_2992,h_1683,c_limit/Desierto-de-Atacama.jpg')"
    }

    function fotoCentro() {
        document.getElementById("div_fotos").style.backgroundImage = "url('https://cdn.wallpapersafari.com/34/91/AhduLG.jpg')"
    }

    function fotoSur() {
        document.getElementById("div_fotos").style.backgroundImage = "url('https://www.navimag.com/hubfs/sur-de-chile-vs-norte-de-chile.jpg')"
    }

    function fotoRapa() {
        document.getElementById("div_fotos").style.backgroundImage = "url('https://i.pinimg.com/originals/b6/7e/46/b67e46db45a8481dd051d3826175c8dd.jpg')"
    }
    return (
        <>
            <div onLoad={cargar}>
                <div className="text text-center" class="titulo"><br></br>
                    <b><h1 style={{ color: "#1687A7" }}>Comienza a buscar tu Departamento ideal</h1></b>
                </div>
                <br />
                <div class="divs">
                    <div class="divs1">
                        <p><h4 style={{ color: "#1687A7" }}>Comuna</h4>
                            <select value={idcomuna} id="comuna" style={{ width: " 200px", height: "34px", borderColor: "#1687A7" }} onChange={cargarComuna}>
                                <option>Comuna</option>
                                {
                                    listaComuna.map(
                                        listaComuna =>
                                            <option key={listaComuna.id_comuna} value={listaComuna.id_comuna}>{listaComuna.nombre_comuna}</option>
                                    )
                                }
                            </select>
                        </p>
                    </div>
                    <div class="divs2">
                        <p><h4 style={{ color: "#1687A7" }}>Fecha de ida</h4>
                            <input type="date" id="fechaReserva" min="2022-11-01" value={fechaIda} onChange={(e) => fechaIdaHandle(e.target.value)}></input>
                        </p>
                    </div>
                    <div class="divs3">
                        <p><h4 style={{ color: "#1687A7" }}>Fecha de vuelta</h4>
                            <input type="date" id="fechaReserva2" min="2022-11-01" value={fechaVuelta} onChange={(e) => fechaVueltaHandle(e.target.value)}></input>
                        </p>
                    </div>
                    <div class="divs3">
                        <p><h4 style={{ color: "#1687A7" }}>Acompañantes</h4>
                            <input type="input" style={{ width: " 200px", height: "32px" }} maxLength={2} value={Acompañantes} onChange={(e) => handleAcompañante(e.target.value)}></input>
                        </p>
                    </div>
                    <button class="button_se"><img src={buscar} height="50" width="50" alt="" onClick={redireccion} /></button>
                </div>
                <br></br>
                <hr style={{ color: "#276678", borderWidth: "5px" }}></hr>
                <div className="text text-center">
                    <br></br>
                    <h2>Comienza tus vacaciones de tus sueños con nosotros</h2>
                </div>
                <br></br>
                <div class="div_principal" className="container" style={{ justifyContent: "center", paddingLeft: "285px" }}>
                    <div className="row">
                        <img src={logo} className="col-lg-2" style={{ width: "285px", height: "110px", paddingTop: "40px" }}></img>
                        <p className="col-lg-2" style={{ width: "500px", textAlign: "justify" }}>La empresa Turismo Real nace con el propósito de lograr las mejores estadías en el territorio nacional, buscando un equilibrio entre la mejor calidad y los
                            precios más accesibles en el mercado actual de turismo. Día a día se agregan nuevos departamentos que cumplen un alto estándar de calidad y son revisados
                            constantemente por el equipo de Turismo Real</p>
                    </div>
                </div>
                <br></br>
                <div id="div_fotos" style={{ maxWidth: "100%" }}>
                    <br></br>
                    <h3 className="text text-left">Los lugares de Chile donde tenemos estadías</h3>
                    <br></br>
                    <br></br>
                    <div id="DIVBTN" style={{width: "80px"}}>
                        <button className="btn btn-primary botones_foto" id="norte" onClick={fotoNorte}>Norte de Chile</button><br></br>
                        <br></br>
                        <br></br>
                        <button className="btn btn-primary botones_foto" id="centro" onClick={fotoCentro}>Zona Central</button><br></br>
                        <br></br>
                        <br></br>
                        <button className="btn btn-primary botones_foto" id="sur" onClick={fotoSur}>Sur de Chile</button><br></br>
                        <br></br>
                        <br></br>
                        <button className="btn btn-primary botones_foto" id="rapanui" onClick={fotoRapa}>Rapa Nui</button><br></br>
                        <br></br>
                        <br></br>            
                    </div>
                </div>
                <br />
            </div>
        </>
    );
}