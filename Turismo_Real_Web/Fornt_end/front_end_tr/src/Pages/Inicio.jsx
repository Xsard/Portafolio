import { CarouselInicio } from "../Components/Carousel/carousel";
import DeptoComponent from '../Components/DeptoComponent/DeptoComponente';
import img_test from '../Img/depto_test.jpg'

export const Inicio = () => {
    return (
        <>
            <div className="text text-center">
                <CarouselInicio></CarouselInicio>
                <br />
                <h1>Departamentos disponibles</h1>
            </div>
            <DeptoComponent></DeptoComponent>
            <br />

        </>
    );
}