package com.turismo.backend_turismo_real.modelo;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

@Entity
@Table(name = "Departamento")
public class Departamento {

	@Id
	@Column(name = "Id_Dpto")
	int IdDepto;
	@Column(name = "Tarifa_Diaria")
	int TarifaDiaria;
	@Column(name = "Direccion")
	String Direccion;
	@Column(name = "Nro_Dpto")
	int NroDepto;
	@Column(name = "Capacidad")
	int Capacidad;
	@Column(name = "Id_Comuna")
	int IdComuna;
	
	
	public int getIdDepto() {
		return IdDepto;
	}
	public void setIdDepto(int idDepto) {
		IdDepto = idDepto;
	}
	public int getTarifaDiaria() {
		return TarifaDiaria;
	}
	public void setTarifaDiaria(int tarifaDiaria) {
		TarifaDiaria = tarifaDiaria;
	}
	public String getDireccion() {
		return Direccion;
	}
	public void setDireccion(String direccion) {
		Direccion = direccion;
	}
	public int getNroDepto() {
		return NroDepto;
	}
	public void setNroDepto(int nroDepto) {
		NroDepto = nroDepto;
	}
	public int getCapacidad() {
		return Capacidad;
	}
	public void setCapacidad(int capacidad) {
		Capacidad = capacidad;
	}
	public int getIdComuna() {
		return IdComuna;
	}
	public void setIdComuna(int idComuna) {
		IdComuna = idComuna;
	}
	public Departamento(int idDepto, int tarifaDiaria, String direccion, int nroDepto, int capacidad, int idComuna) {
		super();
		IdDepto = idDepto;
		TarifaDiaria = tarifaDiaria;
		Direccion = direccion;
		NroDepto = nroDepto;
		Capacidad = capacidad;
		IdComuna = idComuna;
	}
	public Departamento() {
		
	}
	
}
