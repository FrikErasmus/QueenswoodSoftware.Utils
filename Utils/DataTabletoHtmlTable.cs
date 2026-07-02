namespace QueenswoodSoftware.Utils.Utils;

public class DataTableToHtmlTable
{
    public static string DataTableToHtml(DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0)
            return "<p>No data available.</p>";

        var sb = new StringBuilder();
        sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;width:100%;font-family:Arial;font-size:13px;'>");

        // --- HEADER ---
        sb.Append("<thead><tr style='background-color:#f2f2f2;'>");
        foreach (DataColumn col in dt.Columns)
        {
            sb.AppendFormat("<th style='text-align:left;padding:8px;border:1px solid #ddd;'>{0}</th>", System.Net.WebUtility.HtmlEncode(col.ColumnName));
        }
        sb.Append("</tr></thead>");

        // --- BODY ---
        sb.Append("<tbody>");
        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<tr>");
            foreach (var cell in row.ItemArray)
            {
                sb.AppendFormat("<td style='padding:8px;border:1px solid #ddd;'>{0}</td>",
                    System.Net.WebUtility.HtmlEncode(cell?.ToString() ?? string.Empty));
            }
            sb.Append("</tr>");
        }
        sb.Append("</tbody></table>");

        return sb.ToString();
    }
}