function SendEmail($Server, $Port, $Senderz, $Recipient, $Subject, $Body) {

    $SMTPClient = New-Object Net.Mail.SmtpClient($Server, $Port)
    $SMTPClient.EnableSsl = $true
    $SMTPClient.Credentials = New-Object System.Net.NetworkCredential("AKIAUO46JKWBPRWSACHO", "BC0IMFPqvkjujASG1ZCdy4a7zmB2N9l3NVMwIpJl6Lcu");

    try {
        Write-Output "Sending message..."
        $SMTPClient.Send($Senderz, $Recipient, $Subject, $Body)
        Write-Output "Message successfully sent to $($Recipient)"
    } catch [System.Exception] {
        Write-Output "An error occurred:"
        Write-Error $_
    }
}

function SendTestEmail(){
    $Server = "email-smtp.us-east-1.amazonaws.com"
    $Port = 587

    $Subject = "Test email sent from Amazon SES"
    $Body = "This message was sent from Amazon SES using PowerShell (explicit SSL, port 587)."

    $Senderz = "paul.e.gradie@gmail.com"
    $Recipient = "paul.e.gradie@gmail.com"

    SendEmail $Server $Port $Senderz $Recipient $Subject $Body
}

SendTestEmail