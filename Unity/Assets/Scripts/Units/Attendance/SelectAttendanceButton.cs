using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAttendanceButton : MonoBehaviour
{
    [SerializeField]
    private LocalizationText nameText;

    private int attendanceId;
    public SelectAttendancePanelUI parent;
    public void Initialize(AttendanceTable.Data data)
    {
        attendanceId = data.ID;
        nameText.SetString(data.NameStringID);
    }
    public void OnClickSelectAttendanceButton()
    {

    }
}
