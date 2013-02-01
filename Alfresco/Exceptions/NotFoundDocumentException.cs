using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

public class NotFoundDocumentException : ApplicationException
{

	public string Id;

	public string Hipervinculo;
	public NotFoundDocumentException(string Id, string Hipervinculo)
	{
		this.Id = Id;
		this.Hipervinculo = Hipervinculo;
	}

	public NotFoundDocumentException(string Id, string Hipervinculo, string Mensaje)
		: base(Mensaje)
	{
		this.Id = Id;
		this.Hipervinculo = Hipervinculo;
	}

}
