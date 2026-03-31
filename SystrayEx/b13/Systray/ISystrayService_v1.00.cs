// Copyright 2026, Patrice CHARBONNEAU
//                 a.k.a. Sigma3Wolf
//                 oIId: v2.00/2032/160e0e6a3176a8c4235332aa8e0d422c
//                 All rights reserved.
//                 https://b13.ca/
//
// This source code is licensed under the [BSD 3-Clause "New" or "Revised" License] found
// in the LICENSE file in the root directory of this source tree.

#region Usage and dependency
//*************************************************************************************************//
//** WARNING: If you modify this file, you MUST rename it to exclude the version number :WARNING **//
//*************************************************************************************************//
//      Usage:  Wrapper for Nuget Systray package, to be used in Windows Form App
// Dependency:  SystrayApp
#endregion Usage and dependency

#region History
//    History:
// v1.00 - 2026-03-30:	Initial release;
#endregion History

#region b13 namespace
#pragma warning disable IDE0130
namespace b13;
#pragma warning restore IDE0130
#endregion b13 namespace

public interface ISystrayService {
    void ChangeIcon(int plngIconIndex, string pstrMessage = "");
    
    void ShowMenu();

    void Exit();
}

public interface ISystrayAware {
    void SetSystrayService(ISystrayService pobjService);
}