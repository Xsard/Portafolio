package com.turismo.backend_turismo_real.modelo;

import java.util.Date;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table(name = "Reserva")
public class Reserva {
	
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	int id_reserva;
	@Column(name = "id_dpto")
	int id_dpto;
	@Column(name = "id_cliente")
	int id_cliente;
	@Column(name = "estado_reserva")
	String estado_reserva;
	@Column(name = "estado_pago")
	String estado_pago;
	@Column(name = "check_in")
	String check_in;
	@Column(name = "check_out")
	String check_out;
	@Column(name = "firma")
	int firma;
	@Column(name = "valor_total")
	int valor_total;
	public int getId_reserva() {
		return id_reserva;
	}
	public void setId_reserva(int id_reserva) {
		this.id_reserva = id_reserva;
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
	public String getEstado_reserva() {
		return estado_reserva;
	}
	public void setEstado_reserva(String estado_reserva) {
		this.estado_reserva = estado_reserva;
	}
	public String getEstado_pago() {
		return estado_pago;
	}
	public void setEstado_pago(String estado_pago) {
		this.estado_pago = estado_pago;
	}
	public String getCheck_in() {
		return check_in;
	}
	public void setCheck_in(String check_in) {
		this.check_in = check_in;
	}
	public String getCheck_out() {
		return check_out;
	}
	public void setCheck_out(String check_out) {
		this.check_out = check_out;
	}
	public int getFirma() {
		return firma;
	}
	public void setFirma(int firma) {
		this.firma = firma;
	}
	public int getValor_total() {
		return valor_total;
	}
	public void setValor_total(int valor_total) {
		this.valor_total = valor_total;
	}
	public Reserva(int id_reserva, int id_dpto, int id_cliente, String estado_reserva, String estado_pago,
			String check_in, String check_out, int firma, int valor_total) {
		super();
		this.id_reserva = id_reserva;
		this.id_dpto = id_dpto;
		this.id_cliente = id_cliente;
		this.estado_reserva = estado_reserva;
		this.estado_pago = estado_pago;
		this.check_in = check_in;
		this.check_out = check_out;
		this.firma = firma;
		this.valor_total = valor_total;
	}
	public Reserva() {
		
	}

	
}
