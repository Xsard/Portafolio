package com.turismo.backend_turismo_real.modelo;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table(name = "Reserva_servicios_extras")
public class ServExtraReserva {
	
	
	@Column(name = "id_reserva")
	int id_reserva;
	@Id
	@Column(name = "id_svc_ex")
	int id_svc_ex;
	@Column(name = "id_dpto")
	int id_dpto;
	@Column(name = "id_cliente")
	int id_cliente;
	public int getId_reserva() {
		return id_reserva;
	}
	public void setId_reserva(int id_reserva) {
		this.id_reserva = id_reserva;
	}
	public int getId_svc_ex() {
		return id_svc_ex;
	}
	public void setId_svc_ex(int id_svc_ex) {
		this.id_svc_ex = id_svc_ex;
	}
	public int getId_dpto() {
		return id_dpto;
	}
	public void setId_dpto(int id_dpto) {
		this.id_dpto = id_dpto;
	}
	public int getId_cliente() {
		return id_cliente;
	}
	public void setId_cliente(int id_cliente) {
		this.id_cliente = id_cliente;
	}
	public ServExtraReserva(int id_reserva, int id_svc_ex, int id_dpto, int id_cliente) {
		super();
		this.id_reserva = id_reserva;
		this.id_svc_ex = id_svc_ex;
		this.id_dpto = id_dpto;
		this.id_cliente = id_cliente;
	}
	
	public ServExtraReserva() {
		
	}
}
