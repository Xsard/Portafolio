import { useContext } from "react";
import { CarouselInicio } from "../Components/Carousel/carousel";
import DeptoComponent from '../Components/DeptoComponent/DeptoComponente';
import clienteContext from "../Contexts/ClienteContext";
import "../Pages/Inicio.css"

export const Inicio = () => {
    const {usuario, setUsuario} = useContext(clienteContext);
    return (
        <>
        
            <div className="text text-center" class="titulo">
                <br />
                <b><h1 style={{color: "#00ADB5"}}>Departamentos disponibles</h1></b>
            </div>
            <DeptoComponent></DeptoComponent>
            <br />

        </>
    );
}