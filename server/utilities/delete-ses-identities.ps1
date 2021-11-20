

$allIdentities = aws ses list-identities
$total = $allIdentities.Count;

for ($index = 1 ; $index -le $total ; $index++) {
    $cur = $allIdentities[$index]

    if ($cur.Length -eq 29) {

        $charArray = $cur.Split()
        $identity = $charArray[1..($charArray.count)]
        Write-Host $identity
        aws ses delete-identity --identity $identity
    }
}