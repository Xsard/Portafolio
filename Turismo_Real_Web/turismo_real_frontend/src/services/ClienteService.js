import axios from "axios";

const Cliente_Rest_Api_Url = "http://localhost:8080/api/v1/clientes";

class clienteService {

    getCliente(){
        return axios.get(Cliente_Rest_Api_Url);
    }
}

export default new clienteService()