export function createGuid(): string {
	const firstNum: number = Math.round(Math.random() * 10000000)
	const secondNum: number = Math.random() * 10000000
	const concatedGuid: string = firstNum.toString(16) + secondNum.toString(16)
	const cleanGuid: string = concatedGuid.replace(/\W/g, "")
	return `${cleanGuid.substr(0, 6)}-${cleanGuid.substr(6, 5)}-${cleanGuid.substr(10)}`
}

export function processErrors(errors: Record<string, string[]>, ...expectedKeys: string[]): Record<string, string[]> {
	const newErrors: Record<string, string[]> = {}
	Object.entries(errors).forEach(entry => {
		if (expectedKeys.includes(entry[0]))
			newErrors[entry[0]] = entry[1]
		else {
			if (!newErrors["Unknown"])
				newErrors["Unknown"] = []
			newErrors["Unknown"].push(...entry[1])
		}
	})

	return newErrors
}

export function getDocumentTitle(title: string): string {
	return `${title} | Bastion of Shadows`
}