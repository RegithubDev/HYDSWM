using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMMON
{
    public class StoredProcedureHelper
    {

        #region UserManagement
        public const string spGetValidateLogin = "[dbo].[spGetValidateLogin]";
        // public const string spGetValidateLogin = "[dbo].[spGetValidateLogin] '{0}','{1}','{2}','{3}'";
        public const string spGetEXTValidateLogin = "[dbo].[spGetEXTValidateLogin] '{0}','{1}','{2}','{3}'";
        public const string spValidateLoginWithQr = "[dbo].[spValidateLoginWithQr] '{0}','{1}','{2}','{3}','{4}','{5}'";
        public const string spValidateLVehicleWithQr = "[dbo].[spValidateLVehicleWithQr] '{0}','{1}','{2}','{3}'";
        public const string spUserLogout = "[dbo].[spUserLogout] '{0}','{1}','{2}'";
        public const string GetAllUsers = "[dbo].[sp_GetAllUsers] '{0}','{1}','{2}','{3}','{4}','{5}'";
        public const string spGetAllRoles = "[dbo].[spGetAllRoles] '{0}'";
        public const string spGetAllUserLog = "[dbo].[spGetAllUserLog]";
        public const string GetUserDataById = "[dbo].[GetUserDataById] {0}";
        public const string spGetDeployInchargeDataById = "[dbo].[spGetDeployInchargeDataById]";
        public const string spGetMenuByRole = "[dbo].[spGetMenuByRole] '{0}'";
        public const string spGetAllSubMenuByRole = "[dbo].[spGetAllSubMenuByRole] '{0}'";
        public const string spGetAllSubMenuByRoleV1 = "[dbo].[spGetAllSubMenuByRoleV1] '{0}'";
        public const string spGetALLMenuMaster = "[dbo].[spGetALLMenuMaster] {0}";
        public const string spGetALLCircleMaster = "[dbo].[spGetALLCircleMaster] '{0}'";
        public const string spGetAllUZone = "[dbo].[spGetAllUZone] '{0}'";
        public const string spGetAllSubMenuMaster = "[dbo].[spGetAllSubMenuMaster] {0}";
        public const string spGetAllCircleWardMaster = "[dbo].[spGetAllCircleWardMaster] '{0}'";
        public const string spGetAllDeployLocationMaster = "[dbo].[spGetAllDeployLocationMaster]";
        public const string spChangeUserPassword = "[dbo].[spChangeUserPassword] '{0}','{1}','{2}'";
        public const string spSaveNupdateRole = "[dbo].[spSaveNupdateRole]";
        public const string spSaveNupdateMobileRole = "[dbo].[spSaveNupdateMobileRole]";
        public const string spChangeInchargePassword = "[dbo].[spChangeInchargePassword]";
        public const string spGetAllActiveUser = "[dbo].[spGetAllActiveUser]";
        public const string SaveandupdateUser = "[dbo].[sp_InsertOrUpdateUser]";
        public const string sp_InsertOrUpdateDeployeIncharge = "[dbo].[sp_InsertOrUpdateDeployeIncharge]";
        public const string spGetAllMobileMenuSMenuByRoleId = "[dbo].[spGetAllMobileMenuSMenuByRoleId]";
        public const string sp_GetAllTransferStation = "[dbo].[sp_GetAllTransferStation]";
        public const string spGetAllTransferStationByWardId = "[dbo].[spGetAllTransferStationByWardId]";
        public const string spGetAllMobileMenu = "[dbo].[spGetAllMobileMenu]";
        public const string spGetAllMobileSubMenuByRole = "[dbo].[spGetAllMobileSubMenuByRole]";

        #endregion


        #region Asset
        public const string spAddAssetInfo = "[dbo].[spAddAssetInfo] '{0}','{1}','{2}','{3}','{4}',N'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}'";
        public const string spAddAndUpdateAssetInfo = "[dbo].[spAddAndUpdateAssetInfo] '{0}','{1}','{2}','{3}','{4}','{5}',N'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}'";
        public const string spGetAllAsset_Paging = "[dbo].[spGetAllAsset_Paging] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'";
        public const string spGetAssetById = "[dbo].[spGetAssetById] '{0}'";
        public const string spGetAssetInfoByMaskId = "[dbo].[spGetAssetInfoByMaskId] '{0}','{1}'";
        public const string spGetAssetNotification = "[dbo].[spGetAssetNotification] '{0}'";
        public const string spGetAllAssetType = "[dbo].[spGetAllAssetType] '{0}','{1}'";
        public const string spGetAllAssetArea = "[dbo].[spGetAllAssetArea] '{0}','{1}'";

        public const string spGetAllAssetMasterData = "[dbo].[spGetAllAssetMasterData] '{0}','{1}'";

        public const string spGetAssetStatusByCategory = "[dbo].[spGetAssetStatusByCategory] '{0}','{1}','{2}','{3}','{4}'";
        public const string spGetAllAssetBrand = "[dbo].[spGetAllAssetBrand] '{0}'";
        public const string spGetAllAssetMakeModel = "[dbo].[spGetAllAssetMakeModel] '{0}'";
        public const string spGetAllAssetSubcategoryByCategory = "[dbo].[spGetAllAssetSubcategoryByCategory] '{0}'";
        public const string spGetAssetActionStatusById = "[dbo].[spGetAssetActionStatusById] '{0}','{1}'";
        public const string spDeleteAssetInfoById = "[dbo].[spDeleteAssetInfoById] '{0}','{1}','{2}'";
        #endregion
        
        #region SWM Master

        public const string spGetAllCircle = "[dbo].[spGetAllCircle] '{0}','{1}'";
        public const string spGetAllWard = "[dbo].[spGetAllWard] '{0}',{1}";
        public const string spGetAllDesignation = "[dbo].[spGetAllDesignation] '{0}','{1}'";
        public const string spGetAllDesignationFromEmpTbl = "[dbo].[spGetAllDesignationFromEmpTbl]";
        public const string spGetAllCircleByUser = "[dbo].[spGetAllCircleByUser] '{0}'";
        public const string spGetAllWardByUser = "[dbo].[spGetAllWardByUser] '{0}','{1}'";
        public const string spGetAllShift = "[dbo].[spGetAllShift] '{0}','{1}'";
        public const string spGetAllShiftInfo = "[dbo].[spGetAllShiftInfo] '{0}'";
        public const string spGetShiftById = "[dbo].[spGetShiftById] '{0}'";
        //public const string spAddAndUpdateShift = "[dbo].[spAddAndUpdateShift] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'";
        #endregion

        #region HYD Master
        public const string spAddAndUpdateShift = "[dbo].[spAddAndUpdateShift]";
        public const string spGetAllZone = "[dbo].[spGetAllZone]";
        public const string spGetAllCity = "[dbo].[spGetAllCity]";
        public const string spGetAllWardFirst = "[dbo].[spGetAllWardFirst]";
         public const string SelectCircleByWard = "[dbo].[SelectCircleByWard]";
        public const string SelectZoneByWard = "[dbo].[SelectZoneByWard]";

        public const string spGetCityInfoById = "[dbo].[spGetCityInfoById]";
        
        public const string spGetAllDIncharge = "[dbo].[spGetAllDIncharge]";
        public const string spGetAllDLocation = "[dbo].[spGetAllDLocation]";
        public const string spGetAllTrip = "[dbo].[spGetAllTrip]";
        public const string spGetAllActiveVehicle = "[dbo].[spGetAllActiveVehicle]";
        public const string spGetAllActiveGeoPoint = "[dbo].[spGetAllActiveGeoPoint]";
        public const string spGetZoneInfoById = "[dbo].[spGetZoneInfoById]";
        public const string spSaveAndUpdateCity = "[dbo].[spSaveAndUpdateCity]";
        public const string spSaveAndUpdateZone = "[dbo].[spSaveAndUpdateZone]";
        public const string spGetAllCircleByZone = "[dbo].[spGetAllCircleByZone]";
        public const string spGetAllActiveRoute = "[dbo].[spGetAllActiveRoute]";
        public const string spGetMAllCircle = "[dbo].[spGetAllCircle]";
        public const string spGetCircleInfoById = "[dbo].[spGetCircleInfoById]";
        public const string spSaveAndUpdateCircle = "[dbo].[spSaveAndUpdateCircle]";
        public const string spGetAllMWard = "[dbo].[spGetAllWard]";
        public const string spGetWardInfoById = "[dbo].[spGetWardInfoById]";
        public const string spSaveAndUpdateWard = "[dbo].[spSaveAndUpdateWard]";
        public const string spGetAllTransferStation_Paging = "[dbo].[spGetAllTransferStation_Paging]";
        public const string spGetTStationInfoById = "[dbo].[spGetTStationInfoById]";
        public const string spSaveAndUpdateTStationInfo = "[dbo].[spSaveAndUpdateTStationInfo]";
        public const string spGetAllContainerTypeInfo = "[dbo].[spGetAllContainerTypeInfo]";
        public const string spGetAllAssetStatus = "[dbo].[spGetAllAssetStatus]";
        public const string spGetAllVehicleType = "[dbo].[spGetAllVehicleType]";
        public const string spGetVehicleTypeInfoById = "[dbo].[spGetVehicleTypeInfoById]";
        public const string spSaveAndUpdateVehicleType = "[dbo].[spSaveAndUpdateVehicleType]";
        public const string spGetAllOwnerTypeInfo = "[dbo].[spGetAllOwnerTypeInfo]";
        public const string spGetAllOperationType = "[dbo].[spGetAllOperationType]";
        public const string SaveAndUpdateOwnerType = "[dbo].[SaveAndUpdateOwnerType]";
        public const string spGetAllActiveRouteTrip = "[dbo].[spGetAllActiveRouteTrip]";
        public const string spGetAllAssignedVehicleNumber = "[dbo].[spGetAllAssignedVehicleNumber]";
        public const string spGetAllVehicleNumber = "[dbo].[spGetAllVehicleNumber]";
        public const string spGetAllAssignedVehicleNumberbyRoteId = "[dbo].[spGetAllAssignedVehicleNumberbyRoteId]";
        public const string spGetAllGreyPoint = "[dbo].[spGetAllGreyPoint]";
        public const string spInsertEmrPointdetail = "[dbo].[spInsertEmrPointdetail]";
        public const string SP_GetEmrCollectionDetails = "[dbo].[SP_GetEmrCollectionDetails]";
        public const string SP_ViewEmrPointDetails = "[dbo].[SP_ViewEmrPointDetails]";

        #endregion

        #region Hyd Asset
        public const string spGetAllContainer_Paging = "[dbo].[spGetAllContainer_Paging]";
        public const string spGetContainerActionStatusById = "[dbo].[spGetContainerActionStatusById]";
        public const string spGetContainerInfoById = "[dbo].[spGetContainerInfoById]";
        public const string spGetContainerInfoByText = "[dbo].[spGetContainerInfoByText]";
        public const string spSaveAndUpdateContainerInfo = "[dbo].[spSaveAndUpdateContainerInfo]";
        public const string spGetAllVehicle_Paging = "[dbo].[spGetAllVehicle_Paging]";
        public const string spGetAllDeployedNotDepVehicle_Paging = "[dbo].[spGetAllDeployedNotDepVehicle_Paging]";
        public const string spGetDeployedVsNotReported_Paging = "[dbo].[spGetDeployedVsNotReported_Paging]";
        public const string spGetAllDeployedVsReported_Paging = "[dbo].[spGetAllDeployedVsReported_Paging]";
        public const string spGetAllDeployedReported_Paging = "[dbo].[spGetAllDeployedReported_Paging]";
        public const string spGetNotDeployedReported_Paging = "[dbo].[spGetNotDeployedReported_Paging]";
        public const string spGetAllReportedUnique_Paging = "[dbo].[spGetAllReportedUnique_Paging]";
        public const string spSaveAndUpdateVehicleInfo = "[dbo].[spSaveAndUpdateVehicleInfo]";
        public const string spGetVehicleInfoById = "[dbo].[spGetVehicleInfoById]";
        public const string spGetVehicleInfoByText = "[dbo].[spGetVehicleInfoByText]";
        public const string spGetVehicleActionStatusById = "[dbo].[spGetVehicleActionStatusById]";
        public const string spGetAllAssetNotification = "[dbo].[spGetAllAssetNotification]";
        public const string spGetZoneWiseVehicle = "[dbo].[spGetZoneWiseVehicle]";
        public const string spGetVehicleMasterSummary = "[dbo].[spGetVehicleMasterSummary]";
        public const string spGetVehicleMasterSummary_New = "[dbo].[spGetVehicleMasterSummary_New]";
        public const string spGetVehicleDeployedNotDepSummary = "[dbo].[spGetVehicleDeployedNotDepSummary]";
        public const string spGetVehicleDeployedNotDepSummary_New = "[dbo].[spGetVehicleDeployedNotDepSummary_New]";
        public const string spGetDeployedVsNotReportedSummary = "[dbo].[spGetDeployedVsNotReportedSummary]";
        public const string spGetDeployedVsNotReportedSummary_New = "[dbo].[spGetDeployedVsNotReportedSummary_New]";
        public const string spGetVehicleDeployedReportedSummary = "[dbo].[spGetVehicleDeployedReportedSummary]";
        public const string spGetVehicleDeployedReportedSummary_New = "[dbo].[spGetVehicleDeployedReportedSummary_New]";
        public const string spGetNotDeployedVsReportedSummary = "[dbo].[spGetNotDeployedVsReportedSummary]";
        #endregion

        #region Hyd Operation
        public const string spAddNewQrTransaction = "[dbo].[spAddNewQrTransaction]";
        public const string spGetAllDeployIncharge_Paging = "[dbo].[spGetAllDeployIncharge_Paging]";
        public const string spGetAssetInfoByQrScan = "[dbo].[spGetAssetInfoByQrScan]";
        public const string spQrCodeScanAtSCTP = "[dbo].[spQrCodeScanAtSCTP]";
        public const string spGetAllQRTransaction_Paging = "[dbo].[spGetAllQRTransaction_Paging]";
        public const string spGetUIDByCode = "[dbo].[spGetUIDByCode]";
        public const string spGetAllCollectionNotification = "[dbo].[spGetAllCollectionNotification]";
        public const string spGetAllOperation1Noti = "[dbo].[spGetAllOperation1Noti]";
        public const string spGetAllScannedVehicleForBarChart = "[dbo].[spGetAllScannedVehicleForBarChart]";
        public const string spGetTransactionParentId = "[dbo].[spGetTransactionParentId]";
        public const string spGetAllQRTransactionNoti_Paging = "[dbo].[spGetAllQRTransactionNoti_Paging]";
        public const string spGetAllOpt1VehicleNoti = "[dbo].[spGetAllOpt1VehicleNoti]";
        public const string spGetAllOpt1HKLNoti = "[dbo].[spGetAllOpt1HKLNoti]";
        public const string spGetAllOpt1ContainerNoti = "[dbo].[spGetAllOpt1ContainerNoti]";
        public const string spGetAllOpt1RCVNoti = "[dbo].[spGetAllOpt1RCVNoti]";
        public const string spGetAllOpt1UNQContainerNoti = "[dbo].[spGetAllOpt1UNQContainerNoti]";
        public const string spGetAllOpt1UNQHKLNoti = "[dbo].[spGetAllOpt1UNQHKLNoti]";
        public const string spGetAllPrimaryVehicleTransactionByContainer = "[dbo].[spGetAllPrimaryVehicleTransactionByContainer]";
        public const string spGetAllPrimaryTransaction_Paging = "[dbo].[spGetAllPrimaryTransaction_Paging]";
        public const string spAddPrimaryTransaction = "[dbo].[spAddPrimaryTransaction]";
        public const string spGetValidatePrimaryVehicleEntry = "[dbo].[spGetValidatePrimaryVehicleEntry]";
        public const string spChangeStatusOfTransaction = "[dbo].[spChangeStatusOfTransaction]";
        public const string spGetAssetInfoByUID = "[dbo].[spGetAssetInfoByUID]";
        public const string spGetPCollectionById = "[dbo].[spGetPCollectionById]";
        public const string spAddWBTransaction = "[dbo].[spAddWBTransaction]";
        public const string spAddBulkWBInfo = "[dbo].[spAddBulkWBInfo]";
        public const string spGetAllMAppNotification = "[dbo].[spGetAllMAppNotification]";
        public const string spGetAllMTransactionNoti = "[dbo].[spGetAllMTransactionNoti]";
        public const string spGetAllForceTransaction_Paging = "[dbo].[spGetAllForceTransaction_Paging]";
        public const string spGetAllWeightBridgeInfo_Paging = "[dbo].[spGetAllWeightBridgeInfo_Paging]";
        public const string spGetAllMScannedVehicle_Paging = "[dbo].[spGetAllMScannedVehicle_Paging]";
        public const string spGetAllTransferStationByUser = "[dbo].[spGetAllTransferStationByUser]";
        public const string spGetMDashboardCount = "[dbo].[spGetMDashboardCount]";
        public const string spGetMAppLogFile = "[dbo].[spGetMAppLogFile]";
        public const string spAddDeployHLInfo = "[dbo].[spAddDeployHLInfo]";
        public const string spValidateHLUId = "[dbo].[spValidateHLUId]";
        public const string spGetAllAvailContainer = "[dbo].[spGetAllAvailContainer]";
        public const string spGetMDashboardLst = "[dbo].[spGetMDashboardLst]";
        public const string spGetOpMapDashboardLst = "[dbo].[spGetOpMapDashboardLst]";
        public const string spGetDataLogByTS = "[dbo].[spGetDataLogByTS]";
        public const string rptGetOperationSummary_Paging = "[dbo].[rptGetOperationSummary_Paging]";
        public const string spGetAllContPendingForHKL = "[dbo].[spGetAllContPendingForHKL]";
        public const string sprptGetAllLHKL = "[dbo].[sprptGetAllLHKL]";
        public const string sp_GetRptVehicleInfo = "[dbo].[sp_GetRptVehicleInfo]";
        public const string spGetValidateHLUId = "[dbo].[spGetValidateHLUId]";
        public const string spGetAllDHLCInfo_Paging = "[dbo].[spGetAllDHLCInfo_Paging]";
        public const string spGetAllDHLCOpt1Info_Paging = "[dbo].[spGetAllDHLCOpt1Info_Paging]";
        public const string spGetAllActiveDHLCInfo = "[dbo].[spGetAllActiveDHLCInfo]";
        public const string spGetAllPendingOperation = "[dbo].[spGetAllPendingOperation]";
        public const string GetEntityInfoByCodeNdQR = "[dbo].[GetEntityInfoByCodeNdQR]";
        public const string GetPVehicleInfoByQR = "[dbo].[GetPVehicleInfoByQR]";
        public const string spGetAllDeployedVehicle = "[dbo].[spGetAllDeployedVehicle]";
        #endregion

        #region Complaint
        public const string spGetAllComplaintCategory = "[dbo].[spGetAllComplaintCategory]";
        public const string spAddStaffComplaint = "[dbo].[spAddStaffComplaint]";
        public const string spGetAllStaffComplaint_Paging = "[dbo].[spGetAllStaffComplaint_Paging]";
        public const string spUpdateStaffComplaint = "[dbo].[spUpdateStaffComplaint]";
        public const string spGetAllComplaintNotification = "[dbo].[spGetAllComplaintNotification]";
        public const string SaveAndUpdateCCategory = "[dbo].[SaveAndUpdateCCategory]";
        public const string spGetComplaintInfoById = "[dbo].[spGetComplaintInfoById]";
        #endregion

        #region Hyd Deployment
        public const string spGetAllNotFilledContainer = "[dbo].[spGetAllNotFilledContainer]";
        public const string spGetAllEntityInfo = "[dbo].[spGetAllEntityInfo]";
        public const string spGetAllVehicleTypeByLogin = "[dbo].[spGetAllVehicleTypeByLogin]";
        public const string spGetAllDeployedEntiy = "[dbo].[spGetAllDeployedEntiy]";
        public const string spGetEntityTrans = "[dbo].[spGetEntityTrans]";
        public const string spDeploydata = "[dbo].[spDeploydata]";
        public const string spVehicleDeploymentData = "[dbo].[spVehicleDeploymentData]";
        public const string spGetAllVMasterSummary_Paging = "[dbo].[spGetAllVMasterSummary_Paging]";
        public const string spDeploymentTSReport = "[dbo].[spDeploymentTSReport]";
        public const string spValidateInchargeLogin = "[dbo].[spValidateInchargeLogin]";
        public const string spGetDeployedLocationByLoginId = "[dbo].[spGetDeployedLocationByLoginId]";
        public const string spGetAllNotFilledHKL = "[dbo].[spGetAllNotFilledHKL]";
        public const string spUpdateContainerLocation = "[dbo].[spUpdateContainerLocation]";
        public const string spUpdateHKLLocation = "[dbo].[spUpdateHKLLocation]";
        public const string spGetAllHLForDLink = "[dbo].[spGetAllHLForDLink]";
        public const string spDelinkHL = "[dbo].[spDelinkHL]";
        public const string spGetAllUnAllocatedHKL = "[dbo].[spGetAllUnAllocatedHKL]";
        public const string spRemoveArvlContainer = "[dbo].[spRemoveArvlContainer]";
        public const string spGetAllHKLForOperation = "[dbo].[spGetAllHKLForOperation]";
        public const string spAddManualHKL = "[dbo].[spAddManualHKL]";
        public const string spAddManualWBRelease = "[dbo].[spAddManualWBRelease]";
        public const string spRemoveArvlHKL = "[dbo].[spRemoveArvlHKL]";
        public const string spAddEntityDeployment = "[dbo].[spAddEntityDeployment]";
        public const string spGetAllDeployedEntity_Paging = "[dbo].[spGetAllDeployedEntity_Paging]";
        public const string spGetAllVDeployed_Paging = "[dbo].[spGetAllVDeployed_Paging]";
        public const string spGetAllSATTrips_Paging = "[dbo].[spGetAllSATTrips_Paging]";
        public const string spAddPVehicleDeployment = "[dbo].[spAddPVehicleDeployment]";
        public const string rptDeployATVsReport = "[dbo].[rptDeployATVsReport]";

        #endregion

        #region Hyd Operation Rpt
        public const string spGetContainerWisePerformance = "[dbo].[spGetContainerWisePerformance]";
        public const string spGHKLWisePerformance = "[dbo].[spGHKLWisePerformance]";
        public const string rptPVehileWiseInfo = "[dbo].[rptPVehileWiseInfo]";
        #endregion

        #region Hyd Collection
        public const string spAddAndUpdateGeoPoint = "[dbo].[spAddAndUpdateGeoPoint]";
        public const string spAddAndUpdateEmergencyPointMaster = "[dbo].[spAddAndUpdateEmergencyPointMaster]";
        public const string spGetAllGeoPointCategory = "[dbo].[spGetAllGeoPointCategory]";
        public const string spGetGeoPointCategoryInfoById = "[dbo].[spGetGeoPointCategoryInfoById]";
        public const string spGetPointsInfoByTripId = "[dbo].[spGetPointsInfoByTripId]";
        public const string spGetNewRouteCode = "[dbo].[spGetNewRouteCode]";
        public const string spSaveAndUpdateGeoPointCategory = "[dbo].[spSaveAndUpdateGeoPointCategory]";
        public const string spGetAllTripMaster = "[dbo].[spGetAllTripMaster]";
        public const string spGetAllTripPoint_Paging = "[dbo].[spGetAllTripPoint_Paging]";
        public const string spGetAllTripPoint_Paging_New = "[dbo].[spGetAllTripPoint_Paging_New]";
        public const string spAddRouteTrip = "[dbo].[spAddRouteTrip]";
        public const string spGetAllRouteTripByRoute = "[dbo].[spGetAllRouteTripByRoute]";
        public const string spAddAndUpdateSRoute = "[dbo].[spAddAndUpdateSRoute]";
        public const string spAddAndUpdateDeployLocation = "[dbo].[spAddAndUpdateDeployLocation]";
        public const string spGetAllSRoute_Paging = "[dbo].[spGetAllSRoute_Paging]";
        public const string spGetSRouteInfoById = "[dbo].[spGetSRouteInfoById]";
        public const string spGetTripMasterInfoById = "[dbo].[spGetTripMasterInfoById]";
        public const string spSaveAndUpdateTripMaster = "[dbo].[spSaveAndUpdateTripMaster]";
        public const string spGetAllGeoPoint_Paging = "[dbo].[spGetAllGeoPoint_Paging]";
        public const string spGetAllDeployLocation_Paging = "[dbo].[spGetAllDeployLocation_Paging]";
        public const string spGetAllEmergencyPoint_Paging = "[dbo].[spGetAllEmergencyPoint_Paging]";
        public const string spGetAllNRouteByVehicle = "[dbo].[spGetAllNRouteByVehicle]";
        public const string spGetAllNRouteByVehicle_1 = "[dbo].[spGetAllNRouteByVehicle_1]";
        public const string spGetGeoPointInfoById = "[dbo].[spGetGeoPointInfoById]";
        public const string spGetDeployLocationInfoById = "[dbo].[spGetDeployLocationInfoById]";
        public const string spGetGeoPointHistoryById = "[dbo].[spGetGeoPointHistoryById]";
        public const string spGetEmergencyPointMasterInfoById = "[dbo].[spGetEmergencyPointMasterInfoById]";
        public const string spGetAllNRoute_Paging = "[dbo].[spGetAllNRoute_Paging]";
        public const string spAddAndUpdateNRoute = "[dbo].[spAddAndUpdateNRoute]";
        public const string spAddRouteStop = "[dbo].[spAddRouteStop]";
        public const string spAddVehicleToTrip = "[dbo].[spAddVehicleToTrip]";
        public const string spGetRouteInfoById = "[dbo].[spGetRouteInfoById]";
        public const string spGetAllNStopByRouteCode = "[dbo].[spGetAllNStopByRouteCode]";
        public const string spGetAllNStopByTripId = "[dbo].[spGetAllNStopByTripId]";
        public const string spGetAllTripByRoute = "[dbo].[spGetAllTripByRoute]";
        public const string spGetAllTransPointByTrip = "[dbo].[spGetAllTransPointByTrip]";
        public const string spGetAllTransPointByTrip_1 = "[dbo].[spGetAllTransPointByTrip_1]";
        public const string spAddPointCollectTrans = "[dbo].[spAddPointCollectTrans]";
        public const string spAddPointCollectTrans_1 = "[dbo].[spAddPointCollectTrans_1]";
        public const string spValidateTripsStartStatus = "[dbo].[spValidateTripsStartStatus]";
        public const string spValidateTripsStartStatus_Shift = "[dbo].[spValidateTripsStartStatus_Shift]";
        public const string spGetAllPointCollectionDetail_Paging = "[dbo].[spGetAllPointCollectionDetail_Paging]";
        public const string spGetAllRouteWiseCollection_Paging = "[dbo].[spGetAllRouteWiseCollection_Paging]";
        public const string spGetAllColPointByRouteDate = "[dbo].[spGetAllColPointByRouteDate]";
        public const string spGetAllLastSevenDaysPoint = "[dbo].[spGetAllLastSevenDaysPoint]";
        public const string InsertAndUpdateGVP = "[dbo].[InsertAndUpdateGVP]";
        public const string spStartTripAndEndTrip = "[dbo].[spStartTripAndEndTrip]";
        public const string spStartTripAndEndTrip_1 = "[dbo].[spStartTripAndEndTrip_1]";
        public const string spGetAllPointForTimeline = "[dbo].[spGetAllPointForTimeline]";
        public const string spGetAllEmergencyPointForTimeline = "[dbo].[spGetAllEmergencyPointForTimeline]";
        public const string spGetAllRouteWiseCltnSummary = "[dbo].[spGetAllRouteWiseCltnSummary]";
        public const string spGetAllGeoPointsVisitSummary_Paging = "[dbo].[spGetAllGeoPointsVisitSummary_Paging]";
        public const string spGetAllGeoPointVisitTagByPointId = "[dbo].[spGetAllGeoPointVisitTagByPointId]";
        public const string spGetAllPointNoti = "[dbo].[spGetAllPointNoti]";
        public const string sp_GetAllCountGeoCollectionCount = "[dbo].[sp_GetAllCountGeoCollectionCount]";
        public const string spGetAllGVPPoint_Paging = "[dbo].[spGetAllGVPPoint_Paging]";
        public const string spGetAllZoneWiseCollectionNoti = "[dbo].[spGetAllZoneWiseCollectionNoti]";
        public const string spGetAllEmerGencyPointNoti = "[dbo].[spGetAllEmerGencyPointNoti]";
        public const string spGetAllRTripNoti_Paging = "[dbo].[spGetAllRTripNoti_Paging]";
        public const string spGetAllPointCltNoti_Paging = "[dbo].[spGetAllPointCltNoti_Paging]";
        public const string spGetAllEmerPointCltNoti_Paging = "[dbo].[spGetAllEmerPointCltNoti_Paging]";
        public const string spGetAllEarlyLateCompletion_Paging = "[dbo].[spGetAllEarlyLateCompletion_Paging]";
        public const string spGetAllEarlyCompletionByRouteId = "[dbo].[spGetAllEarlyCompletionByRouteId]";
        public const string spGetAllGeoPointsNotCollected_Paging = "[dbo].[spGetAllGeoPointsNotCollected_Paging]";
        public const string spGetAllVisitSummaryForMap_Paging = "[dbo].[spGetAllVisitSummaryForMap_Paging]";
        public const string spGetEmergencyDataForMap_Paging = "[dbo].[spGetEmergencyDataForMap_Paging]";
        public const string spGetTotalVehicleforindex = "[dbo].[spGetTotalVehicleforindex]";
        public const string spGetTotalDeployedVehicle_Paging = "[dbo].[spGetTotalDeployedVehicle_Paging]";
        public const string spGetTotalDeployedVehicleNotAssigned_Paging = "[dbo].[spGetTotalDeployedVehicleNotAssigned_Paging]";
        public const string spGetAllPointSummary_Paging = "[dbo].[spGetAllPointSummary_Paging]";
        public const string spGetAllZoneWiseSummary_Paging = "[dbo].[spGetAllZoneWiseSummary_Paging]";
        public const string sp_SavegreypointintoActual = "[dbo].[sp_SavegreypointintoActual]";
        public const string sp_DeletegreypointintoActual = "[dbo].[sp_DeletegreypointintoActual]";
        public const string spGetEmrVehicleDetail_Paging = "[dbo].[spGetEmrVehicleDetail_Paging]";
        public const string spGetEmergencyPoints = "[dbo].[spGetEmergencyPoints]";
        public const string spGetAllDPointsName = "[dbo].[spGetAllDPointsName]";
        public const string spGetAllEmergencyDPointsName = "[dbo].[spGetAllEmergencyDPointsName]";
        public const string spGetAllDPointsName1 = "[dbo].[spGetAllDPointsName1]";
        public const string spGetAllPointNameByZoneCircle = "[dbo].[spGetAllPointNameByZoneCircle]";
        public const string spAddEmergencyPointCollectTrans = "[dbo].[spAddEmergencyPointCollectTrans]";
        public const string spGetAllEmergency_PointCollectionDetail_Paging = "[dbo].[spGetAllEmergency_PointCollectionDetail_Paging]";
        public const string spGetEmergencyVehicleInfoById = "[dbo].[spGetEmergencyVehicleInfoById]";

        #endregion
    }
}
