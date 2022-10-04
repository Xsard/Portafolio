package com.turismo.backend_turismo_real.repositorio;


import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.JpaSpecificationExecutor;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.turismo.backend_turismo_real.modelo.Departamento;

@Repository
public interface DepartamentoRepositorio extends JpaRepository<Departamento, Integer>, JpaSpecificationExecutor<Departamento>{
	
	@Query(nativeQuery = true, value= "SELECT * FROM Departamento WHERE Id_Dpto=1")
	String QueryDepto();

}
