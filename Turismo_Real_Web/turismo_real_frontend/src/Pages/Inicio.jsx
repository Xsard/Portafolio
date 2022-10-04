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
            <div className="text text-center">
                <h1>Quienes Somos</h1>
                <br />
                <br />
            </div>
            <div class="container">
                <div class="row">
                    <div class="col">
                    </div>
                    <div class="col">
                        <div class="card mb-4" style={{ width: "650px" }}>
                            <div class="row g-0">
                                <div class="col-md-4">
                                    <img src={img_test} class="img-fluid rounded-start" alt="..." />
                                </div>
                                <div class="col-md-8">
                                    <div class="card-body">
                                        <h5 class="card-title">Card title</h5>
                                        <p class="card-text">This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col">
                        <div class="card mb-3 gx-0" style={{ width: "650px" }}>
                            <div class="row g-0">
                                <div class="col-md-4">
                                    <img src={img_test} class="img-fluid rounded-start" alt="..." />
                                </div>
                                <div class="col-md-8">
                                    <div class="card-body">
                                        <h5 class="card-title">test ola</h5>
                                        <p class="card-text">This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col">
                    </div>
                </div>
                
            </div>

        </>
    );
}