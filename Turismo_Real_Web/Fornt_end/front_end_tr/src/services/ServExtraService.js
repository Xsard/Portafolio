import axios from "axios";

const Depto_Rest_Api_Url = "http://localhost:8080/api/v1/allService";

class ServEXtraService {

    getServExtra(){
        return axios.get(Depto_Rest_Api_Url);
    }
}

export default new ServEXtraService()