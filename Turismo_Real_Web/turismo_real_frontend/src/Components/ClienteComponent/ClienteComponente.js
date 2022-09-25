import React from "react";
import ClienteService from "../../services/ClienteService";

class ClienteComponent extends React.Component {

    constructor(props){
        super(props)
        this.state = {
            clientes:[]
        }
    }

    componentDidMount(){
        ClienteService.getCliente().then((Response) => {
            this.setState({clientes: Response.data})
        });
    }

    render (){
        return (
            <div>
                <h1 className="text-center"> Lista de clientes test</h1>
                <table className="table table table-striped">
                    <thead>
                        <tr>
                            <td>Id cliente</td>
                            <td>Rut Cliente</td>
                            <td>Nombres cliente</td>
                            <td>Apellidos cliente</td>
                            <td>Id Usuario</td>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            this.state.clientes.map(
                                clientes =>
                                <tr key={clientes.id_cliente}>
                                    <td>{clientes.id_cliente}</td>
                                    <td>{clientes.rut_cliente}</td>
                                    <td>{clientes.nombres}</td>
                                    <td>{clientes.apellidos}</td>
                                    <td>{clientes.id_usuario}</td>
                                </tr>
                            )
                        }
                    </tbody>
                </table>

            </div>
        );
    }
}

export default ClienteComponent