import { CarouselInicio } from "../Components/Carousel/carousel";
import DeptoComponent from '../Components/DeptoComponent/DeptoComponente';

export const Inicio = () => {
    return (
        <>
        <div className="text text-center">
            <CarouselInicio></CarouselInicio>
            <br />
            <h1>Departamentos disponibles</h1>
            <DeptoComponent></DeptoComponent>
            <br />
            <h1>Quienes Somos</h1>
        </div>
        </>
    );
}