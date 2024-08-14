function caesarCipher(str, shift, decrypt = false) {
  const s = decrypt ? (26 - shift) % 26 : shift; // Adjust shift for decryption
  const n = s > 0 ? s : 26 + (s % 26); // Handle negative shifts

  return [...str]
    .map((l, i) => {
      const c = str.charCodeAt(i);
      if (c >= 65 && c <= 90) {
        // Uppercase letter
        return String.fromCharCode(((c - 65 + n) % 26) + 65);
      } else if (c >= 97 && c <= 122) {
        // Lowercase letter
        return String.fromCharCode(((c - 97 + n) % 26) + 97);
      } else {
        return l;
      }
    })
    .join("");
}

const encryptedInBase64 = (str) => {
  const encryptStr = caesarCipher(str, 3);
  return Buffer.from(encryptStr).toString("base64");
};

const decryptedInBase64 = (str) => {
  const decryptStr = Buffer.from(str, "base64").toString("ascii");
  return caesarCipher(decryptStr, 3, true);
};

// Example usage:
const message = "Anamul";
const key = 3;

const encryptedMessage = caesarCipher(message, key);
console.log("Encrypted:", encryptedMessage);

const decryptedMessage = caesarCipher(encryptedMessage, key, true);
console.log("Decrypted:", decryptedMessage);

const encryptedMessageInBase64 = encryptedInBase64(message);
console.log("Encrypted in Base64:", encryptedMessageInBase64);

const decryptedMessageInBase64 = decryptedInBase64(encryptedMessageInBase64);
console.log("Decrypted in Base64:", decryptedMessageInBase64);
