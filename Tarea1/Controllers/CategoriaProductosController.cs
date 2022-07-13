using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea1.Models;

namespace Tarea1.Controllers
{
    public class CategoriaProductosController : Controller
    {
        Models.Tarea1Entities2 contexto = new Models.Tarea1Entities2();
        // GET: Productos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validar()
        {
            //Capturar los  valores del formulario
            var email = Request["email"];
            var pass = Request["password"];
            //Buscar los datos en la BD
            var usu = (from u in contexto.Usuario where u.email == email && u.pass == pass select u).FirstOrDefault();
            //Si existen Crear session con los datos
            if(usu!=null)
            {
                Session["Usuario"] = usu;
                 return RedirectToAction("Listar");
            }
            else
            {
                return RedirectToAction("Index");
            }
            //Si no redirecciono al Index con un mensaje
        }

        public ActionResult Listar()
        {
            if(Session["Usuario"]!=null)
            {
                var cat = contexto.CategoriaProductos.ToList();
                ViewBag.Categorias = cat;
                return View(cat);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Eliminar(int id)
        {
            if (Session["Usuario"] != null)
            {
                var cat = (from p in contexto.CategoriaProductos where p.Id == id select p).FirstOrDefault();

                contexto.CategoriaProductos.Remove(cat);

                contexto.SaveChanges();

                return RedirectToAction("Listar");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Guardar()
        {
            string mensaje = "";
            var id = Request["idCategoriaProductos"];
            var nombre = Request["nombre"];


            CategoriaProductos c = new CategoriaProductos();
            c.Id = int.Parse(id);
            c.Nombre = nombre;

            
            try
            {
                contexto.CategoriaProductos.Add(c);
                contexto.SaveChanges();
                mensaje = "Guardado con exito";
            }
            catch(Exception ex)
            {
                mensaje = "Error al guardar el dato";
            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Listar");
        }

        public ActionResult MostrarModificar(int id)
        {
            var cat = (from p in contexto.CategoriaProductos where p.Id == id select p).FirstOrDefault();


            if (cat != null)
            {
                var cate = contexto.CategoriaProductos.ToList();
                ViewBag.Categorias = cate;
                return View(cat);
            }
            else
                return RedirectToAction("Listar");
        }

        public ActionResult GuardarModificar()
        {
            int id = int.Parse(Request["idCategoriaProductos"]);
            var nombre = Request["nombre"];


            CategoriaProductos c = (from pro in contexto.CategoriaProductos where pro.Id == id select pro).FirstOrDefault();
            if(c!=null)
            {
                c.Nombre = nombre;

                contexto.SaveChanges();
            }
            
            return RedirectToAction("Listar");
        }
    }
}