import React from "react";
import ServExtraService from "../../services/ServExtraService";
import { Modal, Button } from "react-bootstrap";


class ServExtraComponente extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            servExtra: []
        }
    }

    componentDidMount() {
        ServExtraService.getServExtra().then((Response) => {
            this.setState({ servExtra: Response.data })
        });
    }

    render() {
        return (
            <>
                
            </>
        )
    }
}
export default ServExtraComponente