import axios from "axios";


class servExtraService{

    getServExtra(){
        
        return axios.get("http://localhost:8080/api/v1/allService");
    }
}

export default new servExtraService()