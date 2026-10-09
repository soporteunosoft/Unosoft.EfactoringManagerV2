using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Unosoft.EFactoringManager.Authorization;

public class EFactoringManagerAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

        context.CreatePermission(PermissionNames.Form_users, L("users.form.users"));
        context.CreatePermission(PermissionNames.Btn_createUsers, L("Crear Usuarios"));
        context.CreatePermission(PermissionNames.Btn_editUsers, L("Editar Usuarios"));
        context.CreatePermission(PermissionNames.Btn_deleteUsers, L("Eliminar Usuarios"));
        context.CreatePermission(PermissionNames.Form_Perfil, L("terceros.tercero-home.form.perfil"));
        context.CreatePermission(PermissionNames.Form_PerfilTodos, L("terceros.tercero-home.form.perfilTodos"));
        context.CreatePermission(PermissionNames.Btn_tercero, L("terceros.tercero.btn.tercero"));
        context.CreatePermission(PermissionNames.Btn_incorporar, L("Incorporar Facturas"));
        context.CreatePermission(PermissionNames.Form_facturaHome, L("factura.factura-home.form.factura-home"));
        context.CreatePermission(PermissionNames.Form_facturaList, L("factura.facturas-list.form.facturas-list"));
        context.CreatePermission(PermissionNames.Btn_facturaFiltro, L("factura.factura-filtro.btn.factura-filtro"));
        context.CreatePermission(PermissionNames.Form_solicitudesList, L("factura.solicitudes-list.form.solicitudes-list"));
        context.CreatePermission(PermissionNames.Form_panelImpresion, L("reportes.panel-impresion.form.panel-impresion"));
        context.CreatePermission(PermissionNames.Menu_usuario, L("usuarios.menu.usuario"));
        context.CreatePermission(PermissionNames.Menu_perfil, L("perfil.menu.perfil"));
        context.CreatePermission(PermissionNames.Menu_factoringE, L("factoringE.menu.factoringE"));
        context.CreatePermission(PermissionNames.MenuItem_incorporarFe, L("incorporarFe.menuItem.incorporarFe"));
        context.CreatePermission(PermissionNames.MenuItem_facturas, L("facturas.menuItem.facturas"));
        context.CreatePermission(PermissionNames.MenuItem_solicitudes, L("solicitudes.menuItem.solicitudes"));
        context.CreatePermission(PermissionNames.MenuItem_reportes, L("reportes.menuItem.reportes"));
        context.CreatePermission(PermissionNames.Form_vinculacionesList, L("vinculacionesList.vinculaciones-home.from.vinculaciones-home"));
        context.CreatePermission(PermissionNames.Btn_firmarAutorizada, L("factura.solicitudes-list.btn.firmar"));
        context.CreatePermission(PermissionNames.Form_documentosIniciales, L("app.documentos-iniciales.form.documentos-iniciales"));
        context.CreatePermission(PermissionNames.Grd_Solicitudes, L("factura.solicitudes-list.grd.solicitudes-list"));
        context.CreatePermission(PermissionNames.Grd_Facturas, L("factura.facturas-list.grd.facturas-list"));
        context.CreatePermission(PermissionNames.Txt_mailTerceros, L("terceros.tercero.txt.mail"));
        context.CreatePermission(PermissionNames.Btn_FirmaElectronica, L("Firmar Documentos"));
        context.CreatePermission(PermissionNames.Cargar_Documentos, L("Cargar / Actualizar Documentos Terceros"));
        context.CreatePermission(PermissionNames.Reportes_Show_listTerceros, L("reportes.show.list-terceros"));
        context.CreatePermission(PermissionNames.SolicitudOperaciones_Show_Filtros, L("solicitudOperaciones.show.filtros"));
        context.CreatePermission(PermissionNames.Menu_gestion, L("perfil.menu.gestion"));
        context.CreatePermission(PermissionNames.MenuItem_solicitudEnd, L("solicitudes.menuItem.solicitudEnd"));
        context.CreatePermission(PermissionNames.MenuItem_validadorOperaciones, L("Validador de Operaciones"));
        context.CreatePermission(PermissionNames.Btn_ReenviarFacturasCufes, L("factura.facturas-list.reenviar-facturas-cufes"));
        context.CreatePermission(PermissionNames.Btn_EnviarFacturas, L("Clientes Enviar Facturas"));
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, EFactoringManagerConsts.LocalizationSourceName);
    }
}
