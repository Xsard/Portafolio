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

export const Inicio = () => {
    const { usuario, setUsuario } = useContext(clienteContext);
    const [listaComuna, setListacomuna] = useState([])
    const [idcomuna, setIdComuna] = useState('')
    const [fecha_actual, setFechaActual] = useState('')
    const [fechaIda, setFechaIda] = useState('');
    const [fechaVuelta, setFechaVuelta] = useState('');

    const fechaIdaHandle = fecha_ida => {
        localStorage.setItem("fecha_ida", fecha_ida)
        setFechaIda(fecha_ida)
    }
    const fechaVueltaHandle = fecha_vuelta => {
        localStorage.setItem("fecha_vuelta", fecha_vuelta)
        setFechaVuelta(fecha_vuelta)
    }

    const cargarComuna = (ev) => {
        setIdComuna(ev.target.value)
        localStorage.setItem("id_com", ev.target.value)
    }

    const redireccion = () =>{
        window.location.replace('/DeptoFiltrado');
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

    return (
        <>
            <div onLoad={cargar}>
                <div className="text text-center" class="titulo"><br></br>
                    <b><h1 style={{ color: "#00ADB5" }}>Comienza a buscar tu Departamento ideal</h1></b>
                </div>
                <br />
                <div class="divs">
                    <div class="divs1">
                        <p><h4 style={{ color: "#00ADB5" }}>Comuna</h4>
                            <select value={idcomuna} id="comuna" style={{width: " 200px", height:"34px", borderColor: "#00ADB5" }} onChange={cargarComuna}>
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
                        <p><h4 style={{ color: "#00ADB5" }}>Fecha de ida</h4>
                            <input type="date" id="fechaReserva" min="2022-11-01" value={fechaIda} onChange={(e) => fechaIdaHandle(e.target.value)}></input>
                        </p>
                    </div>
                    <div class="divs3">
                        <p><h4 style={{ color: "#00ADB5" }}>Fecha de vuelta</h4>
                            <input type="date" id="fechaReserva2" min="2022-11-01"value={fechaVuelta} onChange={(e) => fechaVueltaHandle(e.target.value)}></input>
                        </p>
                    </div>
                    <button class="button_se"><img src={buscar} height="50" width="50" alt="" onClick={redireccion}/></button>
                </div>
                <br></br>
                <hr style={{ color: "#00ADB5", borderWidth:"5px"}}></hr>
                <b><h1 className="text text-center" style={{ color: "#00ADB5" }}>Nuestros departamentos</h1></b>
                <DeptoComponent></DeptoComponent>
                <br />
            </div>
        </>
    );
}