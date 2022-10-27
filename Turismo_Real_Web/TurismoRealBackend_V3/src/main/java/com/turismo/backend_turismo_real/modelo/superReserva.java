package com.turismo.backend_turismo_real.modelo;

import java.util.Date;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

public class superReserva {

	String nombre_dpto; 
	
	Date check_in;

	Date check_out;
	
	char estado_pago;
	
	int valor_total;
	
	public char getEstado_pago() {
		return estado_pago;
	}
	public void setEstado_pago(char estado_pago) {
		this.estado_pago = estado_pago;
	}
	public String getNombre_dpto() {
		return nombre_dpto;
	}
	public void setNombre_dpto(String nombre_dpto) {
		this.nombre_dpto = nombre_dpto;
	}

	public Date getCheck_in() {
		return check_in;
	}
	public void setCheck_in(Date check_in) {
		this.check_in = check_in;
	}
	public Date getCheck_out() {
		return check_out;
	}
	public void setCheck_out(Date check_out) {
		this.check_out = check_out;
	}
	public int getValor_total() {
		return valor_total;
	}
	public void setValor_total(int valor_total) {
		this.valor_total = valor_total;
	}
	
	
	
	public superReserva(Date check_in, Date check_out, int valor_total,
			 String nombre_dpto, char estado_pago) {
		super();

		this.check_in = check_in;
		this.check_out = check_out;
		this.valor_total = valor_total;
		this.nombre_dpto = nombre_dpto;
		this.estado_pago = estado_pago;
	}
	public superReserva() {
		
	}
}
