use std::io::prelude::*;
use std::io::BufReader;
use std::fs::File;


fn main() {
    let file = File::open("D:\\repos\\Advent\\Advent2019\\Rust\\Advent1\\src\\input.txt")
        .expect("can't open the file");

    let input = parse_input(file);

    let mut part1 : u32 = 0;
    let mut part2 : u32 = 0;
    for num in input.iter() {
        let recursive_weights = recurse_calc(*num);

        part1 += recursive_weights[0];
        part2 += recursive_weights.iter().sum::<u32>();
    }

    println!("part1: {}", part1);
    println!("part2: {}", part2);
}

fn parse_input(file : File) -> Vec<u32> {
    let mut result : Vec<u32> = Vec::new();

    let reader = BufReader::new(file);
    for (_, line) in reader.lines().enumerate() {
        let line = line.unwrap();

        let num: u32 = line.trim().parse().unwrap();
        result.push(num);
    }

    result
}

fn recurse_calc(input : u32) -> Vec<u32> {
    let mut result = Vec::new();

    let mut calculated = input;
    loop {
        calculated = calc_weight(calculated);
        if calculated == 0 { break; }
        result.push(calculated);
    }

    result
}

fn calc_weight(input : u32) -> u32 {
    if input < 9 { 0 }
    else { (input / 3) - 2 }
}