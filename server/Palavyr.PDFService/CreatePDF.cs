using System.Collections.Generic;
using System.Text;
using PDFService.Sections;
using PDFService.Sections.Util;
using Server.Domain.Accounts;
using Server.Domain.Configuration.Schemas;


namespace PDFService
{
    public static class PdfGenerator
    {
        public static string GenerateNewPDF(UserAccount userAccount, Area previewData, CriticalResponses response, List<Table> staticTables, List<Table> dynamicTables)
        {
            var previewBuilder = new StringBuilder();

            previewBuilder.Append(@"
                    <!DOCTYPE html>
                    <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title></title>
                        </head>
                        <body>
                            <div>");
            
            previewBuilder.Append(HeaderSection.GetHeader(userAccount, response));
            previewBuilder.Append(AreaTitleSection.GetAreaDisplayTitle(previewData.AreaDisplayTitle));
            previewBuilder.Append(PrologueSection.GetPrologue(previewData.Prologue));
            previewBuilder.Append(TablesSection.GetEstimateTables(staticTables, dynamicTables));
            previewBuilder.Append(EpilogueSection.GetEpilogue(previewData.Epilogue));

            previewBuilder.Append(@"</div></body></html>");
            
            var html = previewBuilder.ToString();
            return html;
        }

        public static string MakeFakePdf()
        {
            return @"
                    <!DOCTYPE html>
                    <html lang='en'>

                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title></title>
                    </head>
                    <body>
                        <div>
                           <section id='HEADER' style='display: inline-block'>
                                <div style='height: 100%; float: left; text-align: center; margin-right: 10mm'>
                                    <img src=''
                                        style='max-width: 250px; max-height: 250px; vertical-align: middle;'>
                                </div>
                                <div style='max-height: 100%; float: right;'>
                                    <table
                                        style='table-layout:auto; width: 100%; border-collapse: collapse; text-align: left;'>
                                        <tbody>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>A reeally long Company Name.</th>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>+61-04-5321-3554</th>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>yours.to.contact@gmail.com</th>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table
                                        style='table-layout:auto; width: 100%; border-collapse:collapse; text-align: left'>
                                        <tbody>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Name:</th>
                                                <td>Some Special Name</td>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Estimate Key:</th>
                                                <td>45234-235234234-f223f2f</td>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Date:</th>
                                                <td>24-Jan-2020</td>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Key info #1: </th>
                                                <td>$345,345.23</td>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Key info #2</th>
                                                <td>Key info provided</td>
                                            </tr>                        <tr>
                                                <th style='padding: 2mm' scope='col'>Key info #2</th>
                                                <td>Key info provided</td>
                                            </tr>
                                            <tr>
                                                <th style='padding: 2mm' scope='col'>Key info #2</th>
                                                <td>Key info provided</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                            <section id='TITLE' style='padding-top: 1mm; text-align: center;margin-bottom: 10mm;'>
                                <h2>Service Area Display Title</h2>
                            </section>
                            <section id='PROLOGUE' style='padding-left: .5in; padding-right: .5in; text-align:justify;margin-bottom: 10mm;'>
                                <p>
                                    This is the lengthy <strong>prologue</strong>. There could be a little... or a lot of text here. So we
                                    should design to accomodate with at least a litle bit of flexibility. Some of these will likely spill
                                    over to two pages anyways, so we don't need to be TOO conservative, but we can at least provide some
                                    space as needed.
                                </p>
                            </section>
                            <section id='TABLES' style='padding-left: .5in; padding-right: .5in;'>
                                <div id='FIRST-TABLE' style='margin-bottom: 10mm'>
                                    <table style='table-layout:auto; width: 100%; border-collapse: collapse; border: 2px solid gray; '>
                                        <caption>Variable estimates determined by your responses</caption>
                                        <thead id='STANDARDTABLEHEAD'>
                                            <tr style='text-align: left;'>
                                                <th style='padding: 5mm' style='padding: 5mm' scope='col'>Service Item</th>
                                                <th style='padding: 5mm' scope='col'>Cost</th>
                                                <th style='padding: 5mm' scope='col'></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr style='background-color: lightgray;'>
                                                <td style='padding: 2mm' scope='col'>Some service item you'd like</td>
                                                <td style='padding: 2mm' scope='col'>$100.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'>Per Person</td>
                                            </tr>
                                            <tr style='background-color: none;'>
                                                <td style='padding: 2mm' scope='col'>Some other service item you'd like</td>
                                                <td style='padding: 2mm' scope='col'>$10.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'></td>
                                            </tr>
                                            <tr style='background-color: lightgray;'>
                                                <td style='padding: 2mm' scope='col'>Your favorite service</td>
                                                <td style='padding: 2mm' scope='col'>$35.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div id='SECOND-TABLE' style='margin-bottom: 10mm'>
                                    <table style='table-layout:auto; width: 100%; border-collapse: collapse; border: 2px solid gray; '>
                                        <caption>Standard Estimates</caption>
                                        <thead id='STANDARDTABLEHEAD'>
                                            <tr style='text-align: left;'>
                                                <th style='padding: 5mm' style='padding: 5mm' scope='col'>Service Item</th>
                                                <th style='padding: 5mm' scope='col'>Cost</th>
                                                <th style='padding: 5mm' scope='col'></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr style='background-color: lightgray;'>
                                                <td style='padding: 2mm' scope='col'>Some service item you'd like</td>
                                                <td style='padding: 2mm' scope='col'>$100.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'>Per Person</td>
                                            </tr>
                                            <tr style='background-color: none;'>
                                                <td style='padding: 2mm' scope='col'>Some other service item you'd like</td>
                                                <td style='padding: 2mm' scope='col'>$10.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'></td>
                                            </tr>
                                            <tr style='background-color: lightgray;'>
                                                <td style='padding: 2mm' scope='col'>Your favorite service</td>
                                                <td style='padding: 2mm' scope='col'>$35.00 AUD</td>
                                                <td style='padding: 2mm' scope='col'></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </section>
                            <section id='EPILOGUE' style='padding-left: .5in; padding-right: .5in; text-align:justify;margin-bottom: 10mm;'>
                                <p>
                                    This is the lengthy <strong>Epilogue</strong>. There could be a little... or a lot of text here. So we
                                    should design to accomodate with at least a litle bit of flexibility.
                                    some of these will likely spill over to two pages anyways, so we don't need to be TOO conservative, but
                                    we can at least provide some space as needed.
                                </p>
                                <p>
                                    There could be a little... or a lot of text here. So we
                                    should design to accomodate with at least a litle bit of flexibility.
                                    some of these will likely spill over to two pages anyways, so we don't need to be TOO conservative, but
                                    we can at least provide some space as needed.
                                </p>
                            </section>
                        </div>
                    </body>
                    </html>
                    ";
        }
    }
}