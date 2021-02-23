export type AddSpeciesModel = {
	name: string,
	pluralName: string,
	description: string,
	forceSensitive: boolean,
	hpCoefficient: number,
	strengthModifier: number,
	dexterityModifier: number,
	constitutionModifier: number,
	intelligenceModifier: number,
	charismaModifier: number,
	wisdomModifier: number,
	awarenessModifier: number
}

export type EditSpeciesModel = {
	id: number
} & AddSpeciesModel

export type SpeciesModel = EditSpeciesModel