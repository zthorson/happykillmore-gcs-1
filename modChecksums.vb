Module modChecksums
    Public Function GetChecksum(ByVal inputString As String) As String
        Dim nCount As Integer
        Dim nHolder As Integer

        nHolder = 0
        For nCount = 1 To Len(inputString)
            nHolder = (nHolder Xor CByte(Asc(Mid(inputString, nCount, 1))))
        Next nCount
        GetChecksum = IIf(Len(Hex(nHolder)) = 1, "0", "") & Hex(nHolder)
    End Function
    Public Function GetFY21APChecksum(ByVal inputString As String) As String
        Dim nLength As Integer
        Dim nCount As Integer
        Dim nA As Long

        nA = 0

        nLength = Len(inputString)
        For nCount = 1 To nLength
            nA = nA + Asc(Mid(inputString, nCount, 1))
        Next nCount

        nA = nA Mod 256
        GetFY21APChecksum = Chr("&h" & Hex(nA).PadLeft(2, "0"))
    End Function
    Public Function GetSiRFChecksum(ByVal inputString As String) As String
        Dim nLength As Integer
        Dim nCount As Integer
        Dim nA As Long

        nA = 0

        nLength = Len(inputString)
        For nCount = 1 To nLength
            nA = nA + Asc(Mid(inputString, nCount, 1))
            nA = nA And ((2 ^ 15) - 1)
        Next nCount

        '    Debug.Print "nA=" & Hex(nA) & ",nB=" & Hex(nB)
        GetSiRFChecksum = Hex(nA).PadLeft(4, "0")
        GetSiRFChecksum = Chr("&h" & Mid(GetSiRFChecksum, 1, 2)) & Chr("&h" & Mid(GetSiRFChecksum, 3, 2))
    End Function
    Public Function GetXbeeChecksum(ByVal inputString As String) As String
        Dim nLength As Integer
        Dim nCount As Integer
        Dim nA As Long

        nA = 0

        nLength = Len(inputString)
        For nCount = 1 To nLength
            nA = nA + Asc(Mid(inputString, nCount, 1))
        Next nCount

        nA = nA And CDec("&HFF")
        nA = CDec("&HFF") - nA
        GetXbeeChecksum = Chr("&h" & Hex(nA).PadLeft(2, "0"))
    End Function
    Public Function GetuBloxChecksum(ByVal inputString As String) As String
        Dim nLength As Integer
        Dim nCount As Integer
        Dim nA As Long
        Dim nB As Long

        nA = 0
        nB = 0

        nLength = Len(inputString)
        For nCount = 1 To nLength
            nA = nA + Asc(Mid(inputString, nCount, 1))
            nB = nB + nA
        Next nCount

        nA = nA And &HFF
        nB = nB And &HFF

        '    Debug.Print "nA=" & Hex(nA) & ",nB=" & Hex(nB)
        GetuBloxChecksum = Chr(nA) & Chr(nB)
    End Function

    Public Function GetuBloxChecksumByte(ByVal inputString As String) As Byte()
        Dim nLength As Integer
        Dim nCount As Integer
        Dim nA As Long
        Dim nB As Long
        Dim arr(0 To 1) As Byte

        nA = 0
        nB = 0

        nLength = Len(inputString)
        For nCount = 1 To nLength
            nA = nA + Asc(Mid(inputString, nCount, 1))
            nB = nB + nA
        Next nCount

        nA = nA And &HFF
        nB = nB And &HFF

        '    Debug.Print "nA=" & Hex(nA) & ",nB=" & Hex(nB)

        arr(0) = CInt(nA)
        arr(1) = CInt(nB)
        Return arr

    End Function
End Module
