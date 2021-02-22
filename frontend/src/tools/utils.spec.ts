import {describe, it, expect} from "@jest/globals"
import {createGuid} from "@/tools/utils"

describe("createGuid", () => {
	it("should return a string", () => {
		const guid = createGuid()
		expect(typeof guid).toBe("string")
	})

	it("should return a unique value", () => {
		const guids: string[] = []
		const addGuids: VoidFunction = () => {
			for (let i = 0; i < 1000; i++) {
				const newGuid = createGuid()
				if (guids.includes(newGuid)) {
					throw new Error("Includes the guid!")
				}
				guids.push(newGuid)
			}
		}

		expect(() => addGuids()).not.toThrow()
	})
})