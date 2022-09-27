package com.turismo.backend_turismo_real.repositorio;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.jpa.repository.query.Procedure;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.turismo.backend_turismo_real.modelo.Cliente;

@Repository
public interface ClienteRepositorio extends JpaRepository<Cliente, Integer>{

	//se crea una funcion para llamar al procedure
	//para poder ocupar cualquier nombre en los procedures se hace lo siguiente
	@Query(value = "{call test1}", nativeQuery = true)
	void ListaProcedure();
	
	@Procedure(name="Spring_Procedure_name")
	String procedureName(@Param("p_id") int p_id);
	
	@Query(value = "SELECT test1 FROM DUAL", nativeQuery = true)
	int nose();

}
