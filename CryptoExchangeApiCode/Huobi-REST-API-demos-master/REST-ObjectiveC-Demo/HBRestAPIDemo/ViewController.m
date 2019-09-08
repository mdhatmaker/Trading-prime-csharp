//
//  ViewController.m
//  HBRestAPIDemo
//
//  Created by 似云悠 on 2017/12/15.
//  Copyright © 2017年 似云悠. All rights reserved.
//

#import "ViewController.h"
#import "SYYHuobiNetHandler.h"

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];

    [SYYHuobiNetHandler requestAccountsWithTag:self succeed:^(id respondObject) {

        NSLog(@"-----%@",respondObject);
    } failed:^(id error) {
        NSLog(@"-----%@",error);
    }];
    
   
}


- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


@end
