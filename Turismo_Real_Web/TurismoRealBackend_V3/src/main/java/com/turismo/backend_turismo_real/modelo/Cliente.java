package com.turismo.backend_turismo_real.modelo;

import javax.persistence.*;


@Entity
@Table(name = "Cliente")
public class Cliente {
	
	@Id
	@Column(name = "Id_Cliente")
	private int Id_Cliente;
	
	@Column(name = "Rut_Cliente")
	private String RutCliente;
	@Column(name = "Nombres_Cliente")
	private String Nombres;
	@Column(name = "Apellidos_Cliente")
	private String Apellidos;
	@Column(name = "Id_Usuario")
	private int IdUsuario;
	
	public int getId() {
		return Id_Cliente;
	}
	public void setId(int id) {
		Id_Cliente = id;
	}
	public String getRutCliente() {
		return RutCliente;
	}
	public void setRutCliente(String rutCliente) {
		RutCliente = rutCliente;
	}
	public String getNombres() {
		return Nombres;
	}
	public void setNombres(String nombres) {
		Nombres = nombres;
	}
	public String getApellidos() {
		return Apellidos;
	}
	public void setApellidos(String apellidos) {
		Apellidos = apellidos;
	}
	public int getIdUsuario() {
		return IdUsuario;
	}
	public void setIdUsuario(int idUsuario) {
		IdUsuario = idUsuario;
	}
	public Cliente(int id, String rutCliente, String nombres, String apellidos, int idUsuario) {
		super();
		Id_Cliente = id;
		RutCliente = rutCliente;
		Nombres = nombres;
		Apellidos = apellidos;
		IdUsuario = idUsuario;
	}
	public Cliente() {
		
	}
	
	
}
